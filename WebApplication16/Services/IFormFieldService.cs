using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFormFieldService
    {
        Task<FormField> AddFieldToFormAsync(int formId, string fieldType);
        Task UpdateFieldOrderAsync(int formId, List<int> fieldIds);
        Task<FormField> GetFieldByIdAsync(int fieldId);
        Task<FormField> UpdateFieldAsync(FormField updatedField); // Signature corrected here
        Task DeleteFieldAsync(int fieldId);
    }
}

