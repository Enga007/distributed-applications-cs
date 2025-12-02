using System;
using Microsoft.AspNetCore.Identity;

namespace SafeBG.Models
{
    public class CommentVote
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public bool IsLike { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

