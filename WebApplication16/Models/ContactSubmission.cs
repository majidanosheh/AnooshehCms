using System.ComponentModel.DataAnnotations;
using WebApplication16.Enums;
using WebApplication16.Models;

namespace WebApplication16.Models
{
    public class ContactSubmission : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        public SubmissionStatus Status { get; set; } = SubmissionStatus.New;
    }
}