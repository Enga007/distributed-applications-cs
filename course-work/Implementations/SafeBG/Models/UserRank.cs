using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SafeBG.Models
{
    public class UserRank
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;

        [Required]
        public int Points { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string LevelName { get; set; } = "Observer";

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
