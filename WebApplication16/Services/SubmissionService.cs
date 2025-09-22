using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.Services.Interfaces;

namespace WebApplication16.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly WebApplication16Context _context;

        public SubmissionService(WebApplication16Context context)
        {
            _context = context;
        }

        public async Task<bool> CreateSubmissionAsync(int formId, IFormCollection formData, string ipAddress)
        {
            var form = await _context.Forms.FindAsync(formId);
            if (form == null) return false;

            var submission = new FormSubmission
            {
                FormId = formId,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            foreach (var key in formData.Keys.Where(k => k != "__RequestVerificationToken"))
            {
                var submissionData = new FormSubmissionData
                {
                    FormSubmissionId = submission.Id,
                    FieldName = key,
                    FieldValue = formData[key]
                };
                _context.FormSubmissionData.Add(submissionData);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // پیاده‌سازی متد جدید برای خواندن لیست ارسال‌ها
        public async Task<IEnumerable<FormSubmission>> GetSubmissionsForFormAsync(int formId)
        {
            return await _context.FormSubmissions
                .Where(s => s.FormId == formId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        // پیاده‌سازی متد جدید برای خواندن جزئیات یک ارسال
        public async Task<FormSubmission> GetSubmissionDetailsAsync(int submissionId)
        {
            return await _context.FormSubmissions
                .Include(s => s.SubmissionData) // Join با جدول داده‌ها
                .Include(s => s.Form)           // Join با جدول فرم برای نمایش نام فرم
                .FirstOrDefaultAsync(s => s.Id == submissionId);
        }

        public Task<FormSubmission> CreateSubmissionAsync(FormSubmission submission)
        {
            throw new NotImplementedException();
        }
    }
}

