using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFormFieldService
    {
        Task<FormField?> AddFieldToFormAsync(int formId, string fieldType);
        Task<FormField?> GetFieldByIdAsync(int fieldId);
        Task<FormField?> UpdateFieldAsync(FormField fieldData);
        Task<bool> DeleteFieldAsync(int fieldId);
        Task UpdateFieldOrderAsync(int formId, List<int> fieldIds);
    }
}





//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication16.Models;

//namespace WebApplication16.Services
//{
//    public interface IFormFieldService
//    {
//        Task<FormField> AddFieldToFormAsync(int formId, string fieldType);
//        Task UpdateFieldOrderAsync(int formId, List<int> fieldIds);
//        Task<FormField> GetFieldByIdAsync(int fieldId);
//        Task<FormField> UpdateFieldAsync(FormField updatedField);
//        Task DeleteFieldAsync(int fieldId);
//        Task<FormField?> UpdateFieldAsync(int id, string label, bool isRequired);

//    }
//}

