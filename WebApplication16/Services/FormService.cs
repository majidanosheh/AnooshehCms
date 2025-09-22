using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task DeleteFormAsync(int id)
        {
            var form = await _context.Forms.FindAsync(id);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _context.Forms.OrderByDescending(f => f.CreatedAt).ToListAsync();
        }

        public async Task<Form> GetFormByIdAsync(int id)
        {
            return await _context.Forms.FindAsync(id);
        }

        public async Task<Form> GetFormByIdWithFieldsAsync(int id)
        {
            return await _context.Forms
                .Include(f => f.FormFields)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        // متد جدید پیاده‌سازی شد
        public async Task<Form> GetFormBySlugAsync(string slug)
        {
            return await _context.Forms
                .Include(f => f.FormFields.OrderBy(ff => ff.Order)) // فیلدها را به ترتیب بارگذاری می‌کنیم
                .FirstOrDefaultAsync(f => f.Slug == slug && f.IsActive); // فقط فرم‌های فعال
        }
        public async Task<Form> GetFormWithSubmissionsAsync(int id)
        {
            return await _context.Forms
                .Include(f => f.Submissions.OrderByDescending(s => s.CreatedAt))
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        public Task<Form?> GetFormWithFieldsAsync(int formId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateFormAsync(Form form)
        {
            _context.Update(form);
            await _context.SaveChangesAsync();
        }


    }
}

