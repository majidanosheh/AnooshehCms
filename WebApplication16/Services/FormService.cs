using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;

namespace WebApplication16.Services
{
    public class FormService : IFormService
    {
        private readonly WebApplication16Context _context;

        public FormService(WebApplication16Context context)
        {
            _context = context;
        }

        public async Task<Form> CreateFormAsync(Form form)
        {
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _context.Forms.OrderByDescending(f => f.CreatedAt).ToListAsync();
        }

        public async Task<Form?> GetFormWithFieldsAsync(int formId)
        {
            return await _context.Forms
                                 .Include(f => f.FormFields.OrderBy(ff => ff.Order))
                                 .FirstOrDefaultAsync(f => f.Id == formId);
        }
    }
}
