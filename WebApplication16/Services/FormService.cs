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

        public async Task<bool> DeleteFormAsync(int formId)
        {
            var form = await _context.Forms.FindAsync(formId);
            if (form == null)
            {
                return false;
            }

            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _context.Forms.OrderByDescending(f => f.CreatedAt).ToListAsync();
        }

        public async Task<Form?> GetFormByIdAsync(int formId)
        {
            return await _context.Forms.FindAsync(formId);
        }

        public async Task<Form?> GetFormByIdWithFieldsAsync(int formId)
        {
            return await _context.Forms
                .Include(f => f.FormFields.OrderBy(ff => ff.Order))
                .FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task<Form?> GetFormBySlugWithFieldsAsync(string slug)
        {
            return await _context.Forms
                .Include(f => f.FormFields.OrderBy(ff => ff.Order))
                .FirstOrDefaultAsync(f => f.Slug == slug);
        }

        public async Task<Form?> GetFormWithSubmissionsAsync(int formId)
        {
            return await _context.Forms
                .Include(f => f.Submissions)
                    .ThenInclude(s => s.SubmissionData)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task UpdateFormAsync(Form form)
        {
            _context.Update(form);
            await _context.SaveChangesAsync();
        }
    }
}




//using Microsoft.EntityFrameworkCore;
//using WebApplication16.Areas.Identity.DataAccess;
//using WebApplication16.Models;

//namespace WebApplication16.Services
//{
//    public class FormService : IFormService
//    {
//        private readonly WebApplication16Context _context;

//        public FormService(WebApplication16Context context)
//        {
//            _context = context;
//        }

//        public async Task<Form> CreateFormAsync(Form form)
//        {
//            _context.Forms.Add(form);
//            await _context.SaveChangesAsync();
//            return form;
//        }

//        public async Task DeleteFormAsync(int id)
//        {
//            var form = await _context.Forms.FindAsync(id);
//            if (form != null)
//            {
//                _context.Forms.Remove(form);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task<IEnumerable<Form>> GetAllFormsAsync()
//        {
//            return await _context.Forms.OrderByDescending(f => f.CreatedAt).ToListAsync();
//        }

//        public async Task<Form?> GetFormByIdAsync(int id)
//        {
//            return await _context.Forms.FindAsync(id);
//        }

//        public async Task<Form?> GetFormBySlugWithFieldsAsync(string slug)
//        {
//            return await _context.Forms
//                .Include(f => f.FormFields.OrderBy(ff => ff.Order))
//                .FirstOrDefaultAsync(f => f.Slug == slug && f.IsActive);
//        }

//        public async Task<Form?> GetFormWithFieldsAsync(int id)
//        {
//            return await _context.Forms
//                .Include(f => f.FormFields.OrderBy(ff => ff.Order))
//                .FirstOrDefaultAsync(f => f.Id == id);
//        }

//        public async Task<Form?> GetFormWithSubmissionsAsync(int id)
//        {
//            return await _context.Forms
//                .Include(f => f.Submissions)
//                .FirstOrDefaultAsync(f => f.Id == id);
//        }

//        public async Task UpdateFormAsync(Form form)
//        {
//            _context.Entry(form).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//        }
//    }
//}

