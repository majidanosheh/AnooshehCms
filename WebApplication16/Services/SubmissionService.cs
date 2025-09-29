using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication16.Models;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebApplication16.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly WebApplication16Context _context;

        public SubmissionService(WebApplication16Context context)
        {
            _context = context;
        }

        /// <summary>
        /// ذخیره سازی داده های فرم ارسال شده به همراه آدرس IP.
        /// </summary>
        public async Task<SubmissionResult> CreateSubmissionAsync(int formId, List<FormSubmissionData> submissionData, string? ipAddress)
        {
            if (submissionData == null || !submissionData.Any())
            {
                return new SubmissionResult { Success = false, Errors = new List<string> { "داده ای برای ذخیره سازی وجود ندارد." } };
            }

            var submission = new FormSubmission
            {
                FormId = formId,
                IpAddress = ipAddress,
                SubmissionData = submissionData
            };

            try
            {
                _context.FormSubmissions.Add(submission);
                await _context.SaveChangesAsync();
                return new SubmissionResult { Success = true };
            }
            catch (Exception ex)
            {
                // TODO: پیاده سازی دقیق لاگینگ
                return new SubmissionResult { Success = false, Errors = new List<string> { $"خطا در ذخیره سازی داده‌های فرم: {ex.Message}" } };
            }
        }

        /// <summary>
        /// بازیابی تمام ورودی های یک فرم خاص به همراه جزئیات داده ها.
        /// </summary>
        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId)
        {
            return await _context.FormSubmissions
                       .Where(s => s.FormId == formId)
                       .Include(s => s.SubmissionData)
                       .OrderByDescending(s => s.CreatedAt)
                       .AsNoTracking()
                       .ToListAsync();
        }

        /// <summary>
        /// بازیابی جزئیات یک ورودی خاص. (برای رفع خطای عدم پیاده سازی)
        /// </summary>
        public async Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId)
        {
            return await _context.FormSubmissions
                       .Include(s => s.SubmissionData)
                       .AsNoTracking()
                       .FirstOrDefaultAsync(s => s.Id == submissionId);
        }
    }
}
