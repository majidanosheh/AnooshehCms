using System;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class FormSubmission : BaseEntity
    {
        public int FormId { get; set; }
        public Form Form { get; set; }

        public string? IpAddress { get; set; }

        public ICollection<FormSubmissionData> SubmissionData { get; set; } = new List<FormSubmissionData>();
    }
}
