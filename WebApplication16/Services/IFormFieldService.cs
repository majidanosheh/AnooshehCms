using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFormFieldService
    {
        Task<FormField?> AddFieldToFormAsync(int formId, string fieldType);
        Task<bool> DeleteFieldAsync(int fieldId);
        Task<FormField?> GetFieldByIdAsync(int fieldId);
        Task<string?> UpdateFieldAsync(FormField updatedField);
        Task UpdateFieldOrderAsync(int formId, List<int> fieldIds);
    }
}
