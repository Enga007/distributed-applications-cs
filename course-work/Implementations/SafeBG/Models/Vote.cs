using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SafeBG.Models
{
    public class Vote
    {
        public int Id { get; set; }

        [Required]
        public int ReportId { get; set; }
        public Report Report { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;

        [Required]
        public int Value { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
