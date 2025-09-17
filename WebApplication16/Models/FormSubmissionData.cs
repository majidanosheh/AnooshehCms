using WebApplication16.Models;

namespace WebApplication16.Models
{
    public class FormSubmissionData
    {
        public int Id { get; set; } // این مدل از BaseEntity ارث‌بری نمی‌کند

        public int FormSubmissionId { get; set; }
        public FormSubmission FormSubmission { get; set; }

        public string FieldName { get; set; }
        public string? FieldValue { get; set; }
    }
}