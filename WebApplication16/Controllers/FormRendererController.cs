using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WebApplication16.Enums;
using System.Collections.Generic;
using WebApplication16.Models;

namespace WebApplication16.Controllers
{
    public class FormRendererController : Controller
    {
        private readonly IFormService _formService;
        private readonly ISubmissionService _submissionService;
        private readonly IFileStorageService _fileStorageService;

        public FormRendererController(IFormService formService, ISubmissionService submissionService, IFileStorageService fileStorageService)
        {
            _formService = formService;
            _submissionService = submissionService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet("form/{slug}")]
        public async Task<IActionResult> Display(string slug)
        {
            var form = await _formService.GetFormBySlugWithFieldsAsync(slug);
            if (form == null || !form.IsActive)
            {
                return NotFound();
            }
            return View(form);
        }

        [HttpPost("form/{slug}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string slug)
        {
            var form = await _formService.GetFormBySlugWithFieldsAsync(slug);
            if (form == null)
            {
                return NotFound();
            }

            var formCollection = Request.Form;
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var submissionData = new List<FormSubmissionData>();

            // --- مرحله ۱: اعتبارسنجی سمت سرور ---
            foreach (var field in form.FormFields.OrderBy(f => f.Order))
            {
                // اطمینان از نام فیلد منطبق با رندرینگ
                string fieldName = field.Name ?? $"field_{field.Id}";
                if (field.IsRequired)
                {
                    if (field.FieldType == FieldType.FileUpload)
                    {
                        if (Request.Form.Files.FirstOrDefault(f => f.Name == fieldName) == null || Request.Form.Files.FirstOrDefault(f => f.Name == fieldName)?.Length == 0)
                        {
                            ModelState.AddModelError(fieldName, $"فیلد {field.Label} الزامی است.");
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(formCollection[fieldName]))
                    {
                        ModelState.AddModelError(fieldName, $"فیلد {field.Label} الزامی است.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "لطفاً خطاهای موجود در فرم را برطرف کنید.";
                return View("Display", form);
            }

            // --- مرحله ۲: پردازش و ذخیره فایل‌های آپلود شده ---
            foreach (var file in Request.Form.Files)
            {
                var field = form.FormFields.FirstOrDefault(f => f.Name == file.Name && f.FieldType == FieldType.FileUpload);
                if (field != null)
                {
                    var filePath = await _fileStorageService.StoreFileAsync(file, "form_uploads");
                    if (filePath != null)
                    {
                        submissionData.Add(new FormSubmissionData
                        {
                            FieldName = file.Name,
                            FieldValue = filePath,
                            FileName = file.FileName
                        });
                    }
                }
            }

            // --- مرحله ۳: جمع‌آوری داده‌های فیلدهای متنی و سایر فیلدها ---
            var regularFields = form.FormFields.Where(f => f.FieldType != FieldType.FileUpload).OrderBy(f => f.Order);
            foreach (var field in regularFields)
            {
                string fieldName = field.Name ?? $"field_{field.Id}";
                var value = formCollection[fieldName];

                // هندل کردن چک‌باکس‌های تیک‌نخورده (که در FormCollection نمی‌آیند)
                if (field.FieldType == FieldType.Checkbox && !formCollection.ContainsKey(fieldName))
                {
                    value = "false";
                }

                submissionData.Add(new FormSubmissionData
                {
                    FieldName = fieldName,
                    FieldValue = value.ToString()
                });
            }

            // --- مرحله ۴: ارسال داده‌های جمع‌آوری شده به سرویس Submission ---
            var result = await _submissionService.CreateSubmissionAsync(form.Id, submissionData, ipAddress);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "فرم شما با موفقیت ارسال شد.";
                return RedirectToAction("Display", new { slug });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            TempData["ErrorMessage"] = "خطایی در ارسال فرم رخ داد.";
            return View("Display", form);
        }
    }
}
