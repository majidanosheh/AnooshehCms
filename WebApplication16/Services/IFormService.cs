using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFormService
    {
        Task<IEnumerable<Form>> GetAllFormsAsync();
        Task<Form?> GetFormByIdAsync(int formId);
        Task<Form?> GetFormByIdWithFieldsAsync(int formId);
        Task<Form?> GetFormBySlugWithFieldsAsync(string slug);
        Task<Form> CreateFormAsync(Form form);
        Task UpdateFormAsync(Form form);
        Task<bool> DeleteFormAsync(int formId);
        Task<Form?> GetFormWithSubmissionsAsync(int formId);
    }
}
