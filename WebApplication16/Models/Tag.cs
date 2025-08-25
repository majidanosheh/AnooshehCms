using System.ComponentModel.DataAnnotations;
using WebApplication16.Models;

namespace WebApplication16.Models
{
    public class Tag : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Slug { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}