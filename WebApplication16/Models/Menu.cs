using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace WebApplication16.Models
{
    public class Menu:BaseEntity
    {

        [Required]
        [StringLength(100)]
        [Display(Name = "نام منو")]
        public string Name { get; set; } // مثال: "Main Menu"

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}