using WebApplication16.Enums;
using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFormFieldService
    {
        Task<FormField> AddFieldToFormAsync(int formId, FieldType fieldType);
        Task UpdateFieldOrderAsync(int formId, List<int> fieldIds);
        Task<FormField?> GetFieldByIdAsync(int fieldId);
        Task<FormField> UpdateFieldAsync(int fieldId, string label, bool isRequired);
        Task DeleteFieldAsync(int fieldId);
    }
}



//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication16.Models;

//namespace WebApplication16.Services.Interfaces
//{

//    /// <summary>
//    /// سرویس برای مدیریت فیلدهای یک فرم
//    /// </summary>
//    public interface IFormFieldService
//    {
//        /// <summary>
//        /// افزودن یک فیلد جدید به فرم
//        /// </summary>
//        Task<FormField> AddFieldToFormAsync(int formId, FormField field);

//        /// <summary>
//        /// به‌روزرسانی یک فیلد خاص
//        /// </summary>
//        Task<bool> UpdateFieldAsync(FormField field);

//        /// <summary>
//        /// حذف یک فیلد از فرم
//        /// </summary>
//        Task<bool> DeleteFieldAsync(int fieldId);
//    }

//}
