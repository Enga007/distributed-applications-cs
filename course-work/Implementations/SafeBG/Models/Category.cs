using System.ComponentModel.DataAnnotations;

namespace SafeBG.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(400)]
        public string? Description { get; set; }

        [Required]
        public int SeverityLevel { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;


        public bool NewColumnTest { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}

