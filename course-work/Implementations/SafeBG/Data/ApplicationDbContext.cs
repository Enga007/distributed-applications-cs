using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeBG.Models;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<Vote> Votes { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<UserRank> UserRanks { get; set; } = null!;

    public DbSet<CommentVote> CommentVotes { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var seedDate = new DateTime(2025, 1, 1);

        builder.Entity<Category>().HasData(
           

            new Category { Id = 1, Name = "Dangerous Dog", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 2, Name = "Fire Hazard", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 3, Name = "Broken Road", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 4, Name = "Fallen Tree", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 5, Name = "Flood", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 6, Name = "Crime", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 7, Name = "Car Accident", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 8, Name = "Illegal Dumping", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 9, Name = "Noise Disturbance", CreatedAt = seedDate, IsActive = true },
            new Category { Id = 10, Name = "Other", CreatedAt = seedDate, IsActive = true }
        );

        // COMMENT → REPORT
        builder.Entity<Comment>()
            .HasOne(c => c.Report)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        // COMMENT → USER
        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // VOTE → REPORT  (upvote/downvote за сигнали)
        builder.Entity<Vote>()
            .HasOne(v => v.Report)
            .WithMany(r => r.Votes)
            .HasForeignKey(v => v.ReportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Vote>()
            .HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // COMMENTVOTE → COMMENT 
        builder.Entity<CommentVote>()
            .HasOne(cv => cv.Comment)
            .WithMany(c => c.Votes)
            .HasForeignKey(cv => cv.CommentId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<CommentVote>()
            .HasOne(cv => cv.User)
            .WithMany()
            .HasForeignKey(cv => cv.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
