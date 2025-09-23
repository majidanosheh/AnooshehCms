using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication16.Models;
using WebApplication16.ViewModels;

namespace WebApplication16.Services
{
    public interface ISubmissionService
    {
        Task<SubmissionResult> CreateSubmissionAsync(int formId, IFormCollection formData, string? ipAddress);
        Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId);
        Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId);
    }
}

