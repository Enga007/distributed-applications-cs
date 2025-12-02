using System.ComponentModel.DataAnnotations;
using SafeBG.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace SafeBG.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public ReportStatus Status { get; set; } = ReportStatus.New;

        [Required]
        public int CityId { get; set; }
        public City City { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public string CreatedByUserId { get; set; } = null!;
        public IdentityUser CreatedByUser { get; set; } = null!;

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public bool IsActive { get; set; } = true;
        
    }
}
