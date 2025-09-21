using WebApplication16.Models;
namespace WebApplication16.Services.Interfaces
{
    /// <summary>
    /// سرویس برای مدیریت داده‌های ثبت شده توسط کاربران
    /// </summary>
    public interface ISubmissionService
    {
        /// <summary>
        /// ذخیره اطلاعات ارسال شده یک فرم
        /// </summary>
        Task<FormSubmission> CreateSubmissionAsync(FormSubmission submission);

        /// <summary>
        /// دریافت تمام داده‌های ثبت شده برای یک فرم خاص
        /// </summary>
        Task<IEnumerable<FormSubmission>> GetSubmissionsForFormAsync(int formId);

        /// <summary>
        /// دریافت جزئیات کامل یک داده ثبت شده خاص
        /// </summary>
        Task<FormSubmission> GetSubmissionDetailsAsync(int submissionId);
    }
}