using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFormService
    {
        Task<IEnumerable<Form>> GetAllFormsAsync();
        Task<Form?> GetFormWithFieldsAsync(int formId);
        Task<Form> CreateFormAsync(Form form);
        // متدهای آپدیت و حذف در آینده اضافه خواهند شد
    }
}



//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication16.Models;

//namespace WebApplication16.Services.Interfaces
//{
//    /// <summary>
//    /// سرویس برای مدیریت فرم‌ها
//    /// </summary>
//    public interface IFormService
//    {
//        /// <summary>
//        /// دریافت لیست تمام فرم‌ها
//        /// </summary>
//        Task<IEnumerable<Form>> GetAllFormsAsync();

//        /// <summary>
//        /// دریافت یک فرم خاص با استفاده از شناسه آن
//        /// شامل فیلدهای مرتبط
//        /// </summary>
//        Task<Form> GetFormByIdAsync(int formId);

//        /// <summary>
//        /// دریافت یک فرم خاص با استفاده از آدرس URL آن
//        /// </summary>
//        Task<Form> GetFormBySlugAsync(string slug);

//        /// <summary>
//        /// ایجاد یک فرم جدید
//        /// </summary>
//        Task<Form> CreateFormAsync(Form form);

//        /// <summary>
//        /// به‌روزرسانی اطلاعات یک فرم موجود
//        /// </summary>
//        Task<bool> UpdateFormAsync(Form form);

//        /// <summary>
//        /// حذف یک فرم با استفاده از شناسه آن
//        /// </summary>
//        Task<bool> DeleteFormAsync(int formId);
//    }
//}