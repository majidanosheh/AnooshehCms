using System.ComponentModel.DataAnnotations;
using WebApplication16.Models;

namespace WebApplication16.Models
{
    public class Post : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        public string Content { get; set; }

        [StringLength(500)]
        public string? Excerpt { get; set; } // خلاصه کوتاه

        public string? FeaturedImageUrl { get; set; }

        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }

        // --- روابط ---
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}