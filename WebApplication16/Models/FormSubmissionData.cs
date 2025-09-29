namespace WebApplication16.Models
{
    public class FormSubmissionData
    {
        public int Id { get; set; }
        public int FormSubmissionId { get; set; }
        public FormSubmission FormSubmission { get; set; }

        public string FieldName { get; set; }
        public string? FieldValue { get; set; }
        public string? FileName { get; set; } // نام فایل ذخیره شده برای فیلدهای آپلودی
    }
}
