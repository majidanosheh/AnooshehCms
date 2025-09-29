using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication16.Models;
using WebApplication16.ViewModels;

namespace WebApplication16.Services
{
    public interface ISubmissionService
    {
        // FIX: امضای متد با نوع داده جدید List<FormSubmissionData>
        Task<SubmissionResult> CreateSubmissionAsync(int formId, List<FormSubmissionData> submissionData, string? ipAddress);

        Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId);

        // FIX: اضافه کردن متد GetSubmissionDetailsAsync برای رفع خطای عدم پیاده سازی
        Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId);
    }
}



//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication16.Models;
//using WebApplication16.ViewModels;

//namespace WebApplication16.Services
//{
//    public interface ISubmissionService
//    {


//        Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId); // متد مورد نیاز اضافه شد


//        Task<SubmissionResult> CreateSubmissionAsync(int formId, IFormCollection formData, string? ipAddress);
//        Task<FormSubmission?> GetSubmissionByIdAsync(int submissionId);
//        Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId);
//    }
//}



//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication16.Models;
//using WebApplication16.ViewModels;

//namespace WebApplication16.Services
//{
//    public interface ISubmissionService
//    {
//        Task<SubmissionResult> CreateSubmissionAsync(int formId, IFormCollection formData, string? ipAddress);
//        Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId);
//        Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId);
//    }
//}

