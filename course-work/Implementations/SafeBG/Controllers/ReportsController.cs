using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SafeBG.Data;
using SafeBG.Models;
using SafeBG.Models.Enums;
using SafeBG.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SafeBG.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // -------------------------------------------------------
        // INDEX
        // -------------------------------------------------------
        public async Task<IActionResult> Index(ReportFilterViewModel filter)
        {
            filter.Cities = await _context.Cities.OrderBy(c => c.Name).ToListAsync();
            filter.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();

            var query = _context.Reports
                .Include(r => r.City)
                .Include(r => r.Category)
                .Include(r => r.CreatedByUser)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(r =>
                    r.Title.Contains(filter.SearchText) ||
                    r.Description.Contains(filter.SearchText));
            }

            // City
            if (filter.CityId.HasValue)
                query = query.Where(r => r.CityId == filter.CityId.Value);

            // Category
            if (filter.CategoryId.HasValue)
                query = query.Where(r => r.CategoryId == filter.CategoryId.Value);

            // Status
            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            // Sorting
            switch (filter.SortBy)
            {
                case "date":
                    query = filter.SortAsc ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt);
                    break;
                case "city":
                    query = filter.SortAsc ? query.OrderBy(r => r.City.Name) : query.OrderByDescending(r => r.City.Name);
                    break;
                case "category":
                    query = filter.SortAsc ? query.OrderBy(r => r.Category.Name) : query.OrderByDescending(r => r.Category.Name);
                    break;
                default:
                    query = query.OrderByDescending(r => r.CreatedAt);
                    break;
            }

            filter.TotalCount = await query.CountAsync();
            filter.Results = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return View(filter);
        }

        // -------------------------------------------------------
        // DETAILS 
        // -------------------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Reports
                .Include(r => r.City)
                .Include(r => r.Category)
                .Include(r => r.Votes)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Votes)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
                return NotFound();

            return View(report);
        }

        // -------------------------------------------------------
        // CREATE
        // -------------------------------------------------------
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Latitude,Longitude,Address,CityId,CategoryId")] Report report)
        {
            ModelState.Remove("CreatedAt");
            ModelState.Remove("Status");
            ModelState.Remove("CreatedByUserId");
            ModelState.Remove("City");
            ModelState.Remove("Category");
            ModelState.Remove("CreatedByUser");

            if (!ModelState.IsValid)
            {
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", report.CityId);
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", report.CategoryId);
                return View(report);
            }

            report.CreatedAt = DateTime.UtcNow;
            report.Status = ReportStatus.New;
            report.CreatedByUserId = _userManager.GetUserId(User);

            _context.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------
        // ADD COMMENT
        // -------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return RedirectToAction(nameof(Details), new { id });

            var userId = _userManager.GetUserId(User);

            var comment = new Comment
            {
                ReportId = id,
                UserId = userId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        // -------------------------------------------------------
        // VOTE FOR REPORT
        // -------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Vote(int id)
        {
            var userId = _userManager.GetUserId(User);

            bool already = await _context.Votes.AnyAsync(v => v.ReportId == id && v.UserId == userId);

            if (!already)
            {
                _context.Votes.Add(new Vote
                {
                    ReportId = id,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // -------------------------------------------------------
        // VOTE FOR COMMENT (LIKE / DISLIKE)
        // -------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> VoteComment(int commentId, bool isLike)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var comment = await _context.Comments
                .Include(c => c.Votes)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null) return NotFound();

            var existingVote = comment.Votes.FirstOrDefault(v => v.UserId == userId);

            if (existingVote != null)
            {
                existingVote.IsLike = isLike;
            }
            else
            {
                comment.Votes.Add(new CommentVote
                {
                    UserId = userId,
                    CommentId = commentId,
                    IsLike = isLike,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = comment.ReportId });
        }

        // -------------------------------------------------------
        // DELETE, EDIT, etc. 
        // -------------------------------------------------------

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var report = await _context.Reports
                .Include(r => r.City)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
                return NotFound();

            ViewBag.CityId = new SelectList(_context.Cities, "Id", "Name", report.CityId);
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", report.CategoryId);

            return View(report);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Report model)
        {
            // Премахваме ненужните полета от валидацията
            ModelState.Remove("CreatedAt");
            ModelState.Remove("CreatedByUserId");
            ModelState.Remove("Status");
            ModelState.Remove("City");
            ModelState.Remove("Category");
            ModelState.Remove("CreatedByUser");

            // Показваме грешки в Output
            foreach (var error in ModelState)
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CityId = new SelectList(_context.Cities, "Id", "Name", model.CityId);
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            var report = await _context.Reports.FindAsync(id);

            if (report == null)
                return NotFound();

            report.Title = model.Title;
            report.Description = model.Description;
            report.Latitude = model.Latitude;
            report.Longitude = model.Longitude;
            report.CityId = model.CityId;
            report.CategoryId = model.CategoryId;

            // Запазваме същия Status
            report.Status = report.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Reports
                .Include(r => r.City)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null) return NotFound();

            return View(report);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
