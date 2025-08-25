using System.ComponentModel.DataAnnotations;
using WebApplication16.Models;


namespace WebApplication16.Models
{
    // این یک جدول پیوندی (Join Table) است
    public class PostTag : BaseEntity
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}