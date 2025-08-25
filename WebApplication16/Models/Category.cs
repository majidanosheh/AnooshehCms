using System.ComponentModel.DataAnnotations;



namespace WebApplication16.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Slug { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}