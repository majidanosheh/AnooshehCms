using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.ViewModels;

namespace WebApplication16.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly WebApplication16Context _context;

        public SubmissionService(WebApplication16Context context)
        {
            _context = context;
        }

        public async Task<SubmissionResult> CreateSubmissionAsync(int formId, IFormCollection formData, string? ipAddress)
        {
            var result = new SubmissionResult();
            var form = await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == formId);

            if (form == null)
            {
                result.Errors.Add("فرم مورد نظر یافت نشد.");
                return result;
            }

            // --- Validation Logic ---
            foreach (var field in form.FormFields)
            {
                if (string.IsNullOrEmpty(field.Name)) continue;

                if (field.IsRequired && string.IsNullOrEmpty(formData[field.Name]))
                {
                    result.Errors.Add($"فیلد '{field.Label}' الزامی است.");
                }
            }

            if (result.Errors.Any())
            {
                return result;
            }

            // --- Save Submission ---
            var submission = new FormSubmission
            {
                FormId = formId,
                IpAddress = ipAddress
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            foreach (var field in form.FormFields)
            {
                if (string.IsNullOrEmpty(field.Name)) continue;

                var submissionData = new FormSubmissionData
                {
                    FormSubmissionId = submission.Id,
                    FieldName = field.Label,
                    FieldValue = formData[field.Name]
                };
                _context.FormSubmissionData.Add(submissionData);
            }

            await _context.SaveChangesAsync();
            result.Success = true;
            return result;
        }

        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId)
        {
            return await _context.FormSubmissions
                .Where(s => s.FormId == formId)
                .Include(s => s.SubmissionData)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<FormSubmission?> GetSubmissionByIdAsync(int submissionId)
        {
            return await _context.FormSubmissions
                .Include(s => s.SubmissionData)
                .FirstOrDefaultAsync(s => s.Id == submissionId);
        }
    }
}


//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebApplication16.Areas.Identity.DataAccess;
//using WebApplication16.Models;
//using WebApplication16.ViewModels;

//namespace WebApplication16.Services
//{
//    public class SubmissionService : ISubmissionService
//    {
//        private readonly WebApplication16Context _context;

//        public SubmissionService(WebApplication16Context context)
//        {
//            _context = context;
//        }

//        public async Task<SubmissionResult> CreateSubmissionAsync(int formId, IFormCollection formData, string? ipAddress)
//        {
//            var form = await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == formId);
//            if (form == null)
//            {
//                return new SubmissionResult { Success = false, Errors = new List<string> { "فرم مورد نظر یافت نشد." } };
//            }

//            var errors = new List<string>();
//            var submissionData = new List<FormSubmissionData>();

//            foreach (var field in form.FormFields)
//            {
//                if (string.IsNullOrEmpty(field.Name))
//                {
//                    continue;
//                }

//                formData.TryGetValue(field.Name, out var value);
//                string? submittedValue = value.ToString();

//                if (field.IsRequired && string.IsNullOrWhiteSpace(submittedValue))
//                {
//                    errors.Add($"فیلد '{field.Label}' الزامی است.");
//                }

//                submissionData.Add(new FormSubmissionData
//                {
//                    FieldName = field.Name,
//                    FieldValue = submittedValue
//                });
//            }

//            if (errors.Any())
//            {
//                return new SubmissionResult { Success = false, Errors = errors };
//            }

//            var submission = new FormSubmission
//            {
//                FormId = formId,
//                IpAddress = ipAddress,
//                SubmissionData = submissionData
//            };

//            _context.FormSubmissions.Add(submission);
//            await _context.SaveChangesAsync();

//            return new SubmissionResult { Success = true };
//        }

//        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId)
//        {
//            return await _context.FormSubmissions
//               .Where(s => s.FormId == formId)
//               .Include(s => s.SubmissionData)
//               .OrderByDescending(s => s.CreatedAt)
//               .ToListAsync();
//        }

//        public async Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId)
//        {
//            return await _context.FormSubmissions
//                .Include(s => s.SubmissionData)
//                .FirstOrDefaultAsync(s => s.Id == submissionId);
//        }
//    }
//}


////using Microsoft.EntityFrameworkCore;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using WebApplication16.Areas.Identity.DataAccess;
////using WebApplication16.Models;
////using WebApplication16.ViewModels;

////namespace WebApplication16.Services
////{
////    public class SubmissionService : ISubmissionService
////    {
////        private readonly WebApplication16Context _context;

////        public SubmissionService(WebApplication16Context context)
////        {
////            _context = context;
////        }

////        public async Task<SubmissionResult> CreateSubmissionAsync(int formId, IFormCollection formData, string? ipAddress)
////        {
////            var form = await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == formId);
////            if (form == null)
////            {
////                return new SubmissionResult { Success = false, Errors = new List<string> { "فرم مورد نظر یافت نشد." } };
////            }

////            var errors = new List<string>();
////            var submissionData = new List<FormSubmissionData>();

////            foreach (var field in form.FormFields)
////            {
////                formData.TryGetValue(field.Name!, out var value);
////                string? submittedValue = value.ToString();

////                if (field.IsRequired && string.IsNullOrWhiteSpace(submittedValue))
////                {
////                    errors.Add($"فیلد '{field.Label}' الزامی است.");
////                }

////                submissionData.Add(new FormSubmissionData
////                {
////                    FieldName = field.Name!,
////                    FieldValue = submittedValue
////                });
////            }

////            if (errors.Any())
////            {
////                return new SubmissionResult { Success = false, Errors = errors };
////            }

////            var submission = new FormSubmission
////            {
////                FormId = formId,
////                IpAddress = ipAddress,
////                SubmissionData = submissionData
////            };

////            _context.FormSubmissions.Add(submission);
////            await _context.SaveChangesAsync();

////            return new SubmissionResult { Success = true };
////        }

////        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsWithDataAsync(int formId)
////        {
////            return await _context.FormSubmissions
////               .Where(s => s.FormId == formId)
////               .Include(s => s.SubmissionData)
////               .OrderByDescending(s => s.CreatedAt)
////               .ToListAsync();
////        }

////        public async Task<FormSubmission?> GetSubmissionDetailsAsync(int submissionId)
////        {
////            return await _context.FormSubmissions
////                .Include(s => s.SubmissionData)
////                .FirstOrDefaultAsync(s => s.Id == submissionId);
////        }
////    }
////}



////using Microsoft.EntityFrameworkCore;
////using WebApplication16.Areas.Identity.DataAccess;
////using WebApplication16.Models;
////using WebApplication16.ViewModels;

////namespace WebApplication16.Services
////{
////    public class SubmissionService : ISubmissionService
////    {
////        private readonly WebApplication16Context _context;

////        public SubmissionService(WebApplication16Context context)
////        {
////            _context = context;
////        }

////        public async Task<SubmissionResult> CreateSubmissionAsync(int formId, Dictionary<string, string> formData, string? ipAddress)
////        {
////            var form = await _context.Forms
////                .Include(f => f.FormFields)
////                .AsNoTracking()
////                .FirstOrDefaultAsync(f => f.Id == formId);

////            if (form == null)
////            {
////                return SubmissionResult.Failed("فرم مورد نظر یافت نشد.");
////            }

////            var validationErrors = new List<string>();
////            foreach (var field in form.FormFields)
////            {
////                if (field.IsRequired && (!formData.ContainsKey(field.Name) || string.IsNullOrWhiteSpace(formData[field.Name])))
////                {
////                    validationErrors.Add($"فیلد '{field.Label}' الزامی است.");
////                }
////            }

////            if (validationErrors.Any())
////            {
////                return SubmissionResult.Failed(validationErrors.ToArray());
////            }

////            var submission = new FormSubmission
////            {
////                FormId = formId,
////                IpAddress = ipAddress,
////                CreatedAt = DateTime.UtcNow
////            };

////            foreach (var data in formData)
////            {
////                submission.SubmissionData.Add(new FormSubmissionData
////                {
////                    FieldName = data.Key,
////                    FieldValue = data.Value
////                });
////            }

////            _context.FormSubmissions.Add(submission);
////            await _context.SaveChangesAsync();

////            return SubmissionResult.Success();
////        }

////        public async Task<FormSubmission?> GetSubmissionByIdAsync(int submissionId)
////        {
////            return await _context.FormSubmissions
////                .Include(s => s.SubmissionData)
////                .Include(s => s.Form)
////                .ThenInclude(f => f.FormFields)
////                .AsNoTracking()
////                .FirstOrDefaultAsync(s => s.Id == submissionId);
////        }

////        public async Task<IEnumerable<FormSubmission>> GetSubmissionsByFormIdAsync(int formId)
////        {
////            return await _context.FormSubmissions
////                .Where(s => s.FormId == formId)
////                .Include(s => s.SubmissionData)
////                .AsNoTracking()
////                .OrderByDescending(s => s.CreatedAt)
////                .ToListAsync();
////        }
////    }
////}



//////using Microsoft.EntityFrameworkCore;
//////using WebApplication16.Areas.Identity.DataAccess;
//////using WebApplication16.Models;
//////using WebApplication16.ViewModels;

//////namespace WebApplication16.Services
//////{
//////    public class SubmissionService : ISubmissionService
//////    {
//////        private readonly WebApplication16Context _context;

//////        public SubmissionService(WebApplication16Context context)
//////        {
//////            _context = context;
//////        }

//////        public async Task<SubmissionResult> CreateSubmissionAsync(int formId, Dictionary<string, string> formData, string? ipAddress)
//////        {
//////            var form = await _context.Forms
//////                .Include(f => f.FormFields)
//////                .FirstOrDefaultAsync(f => f.Id == formId);

//////            if (form == null)
//////            {
//////                return SubmissionResult.Failed("فرم مورد نظر یافت نشد.");
//////            }

//////            var validationErrors = new List<string>();
//////            foreach (var field in form.FormFields)
//////            {
//////                if (field.IsRequired && (!formData.ContainsKey(field.Name) || string.IsNullOrWhiteSpace(formData[field.Name])))
//////                {
//////                    validationErrors.Add($"فیلد '{field.Label}' الزامی است.");
//////                }
//////            }

//////            if (validationErrors.Any())
//////            {
//////                return SubmissionResult.Failed(validationErrors.ToArray());
//////            }

//////            var submission = new FormSubmission
//////            {
//////                FormId = formId,
//////                IpAddress = ipAddress,
//////                CreatedAt = DateTime.UtcNow // Use UtcNow for consistency
//////            };

//////            foreach (var data in formData)
//////            {
//////                submission.SubmissionData.Add(new FormSubmissionData
//////                {
//////                    FieldName = data.Key,
//////                    FieldValue = data.Value
//////                });
//////            }

//////            _context.FormSubmissions.Add(submission);
//////            await _context.SaveChangesAsync();

//////            return SubmissionResult.Success();
//////        }

//////        public async Task<FormSubmission?> GetSubmissionByIdAsync(int submissionId)
//////        {
//////            return await _context.FormSubmissions
//////                .Include(s => s.SubmissionData)
//////                .Include(s => s.Form)
//////                .ThenInclude(f => f.FormFields) // Load form fields as well
//////                .FirstOrDefaultAsync(s => s.Id == submissionId);
//////        }

//////        public async Task<IEnumerable<FormSubmission>> GetSubmissionsByFormIdAsync(int formId)
//////        {
//////            return await _context.FormSubmissions
//////                .Where(s => s.FormId == formId)
//////                .Include(s => s.SubmissionData)
//////                .OrderByDescending(s => s.CreatedAt)
//////                .ToListAsync();
//////        }
//////    }
//////}

