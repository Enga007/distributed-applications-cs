using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SafeBG.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int ReportId { get; set; }
        public Report Report { get; set; } = null!;

        public ICollection<CommentVote> Votes { get; set; } = new List<CommentVote>();


        [Required]
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
