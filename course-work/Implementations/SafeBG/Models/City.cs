using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace SafeBG.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Region { get; set; } = null!;

        public int? Population { get; set; }

        [Required]
        public bool HasMetro { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
