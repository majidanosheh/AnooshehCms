using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;
using WebApplication16.Models;
using WebApplication16.ViewModels;
using System.Text;

namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FormsController : Controller
    {
        private readonly IFormService _formService;
        private readonly IFormFieldService _formFieldService;
        private readonly ISubmissionService _submissionService;

        public FormsController(IFormService formService, IFormFieldService formFieldService, ISubmissionService submissionService)
        {
            _formService = formService;
            _formFieldService = formFieldService;
            _submissionService = submissionService;
        }

        // GET: /Admin/Forms
        public async Task<IActionResult> Index()
        {
            var forms = await _formService.GetAllFormsAsync();
            return View(forms);
        }

        // GET: /Admin/Forms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Forms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Slug,IsActive")] Form form)
        {
            if (ModelState.IsValid)
            {
                await _formService.CreateFormAsync(form);
                TempData["SuccessMessage"] = "فرم با موفقیت ایجاد شد.";
                return RedirectToAction(nameof(Design), new { id = form.Id });
            }
            return View(form);
        }

        // GET: /Admin/Forms/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return View(form);
        }

        // POST: /Admin/Forms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Slug,IsActive,CreatedAt,CreatedBy")] Form form)
        {
            if (id != form.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _formService.UpdateFormAsync(form);
                TempData["SuccessMessage"] = "فرم با موفقیت ویرایش شد.";
                return RedirectToAction(nameof(Index));
            }
            return View(form);
        }

        // GET: /Admin/Forms/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return View(form);
        }

        // POST: /Admin/Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _formService.DeleteFormAsync(id);
            TempData["SuccessMessage"] = "فرم با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }


        // GET: /Admin/Forms/Design/5
        public async Task<IActionResult> Design(int id)
        {
            var form = await _formService.GetFormWithFieldsAsync(id);
            if (form == null) return NotFound();
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFieldToForm(int formId, string fieldType)
        {
            var newField = await _formFieldService.AddFieldToFormAsync(formId, fieldType);
            if (newField == null) return BadRequest("نوع فیلد نامعتبر است.");
            return PartialView("_FormFieldItem", newField);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFieldOrder([FromBody] UpdateOrderViewModel model)
        {
            await _formFieldService.UpdateFieldOrderAsync(model.FormId, model.FieldIds);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetField(int id)
        {
            var field = await _formFieldService.GetFieldByIdAsync(id);
            if (field == null) return NotFound();
            return Ok(field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateField([FromBody] FormField updatedField)
        {
            var field = await _formFieldService.UpdateFieldAsync(updatedField);
            if (field == null) return NotFound();
            return PartialView("_FormFieldItem", field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteField(int id)
        {
            var success = await _formFieldService.DeleteFieldAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        // GET: /Admin/Forms/Submissions/5
        public async Task<IActionResult> Submissions(int id)
        {
            var form = await _formService.GetFormWithSubmissionsAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return View(form);
        }

        // GET: /Admin/Forms/SubmissionDetails/5
        public async Task<IActionResult> SubmissionDetails(int id)
        {
            var submission = await _submissionService.GetSubmissionDetailsAsync(id);
            if (submission == null)
            {
                return NotFound();
            }
            return View(submission);
        }

        // GET: /Admin/Forms/ExportToCsv/5
        public async Task<IActionResult> ExportToCsv(int id)
        {
            var form = await _formService.GetFormWithFieldsAsync(id);
            if (form == null) return NotFound();

            var submissions = await _submissionService.GetAllSubmissionsWithDataAsync(id);

            var builder = new StringBuilder();

            // Header
            var headers = form.FormFields.OrderBy(f => f.Order).Select(f => f.Label).ToList();
            headers.Insert(0, "تاریخ ارسال");
            builder.AppendLine(string.Join(",", headers));

            // Rows
            foreach (var sub in submissions)
            {
                var row = new List<string> { sub.CreatedAt.ToString("yyyy-MM-dd HH:mm") };
                foreach (var headerField in form.FormFields.OrderBy(f => f.Order))
                {
                    var data = sub.SubmissionData.FirstOrDefault(d => d.FieldName == headerField.Name);
                    // For CSV, wrap value in quotes to handle commas within the data
                    row.Add($"\"{data?.FieldValue?.Replace("\"", "\"\"") ?? ""}\"");
                }
                builder.AppendLine(string.Join(",", row));
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"form_{id}_submissions.csv");
        }
    }
}



//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Text;
//using WebApplication16.Models;
//using WebApplication16.Services;
//using WebApplication16.ViewModels;

//namespace WebApplication16.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize]
//    public class FormsController : Controller
//    {
//        private readonly IFormService _formService;
//        private readonly IFormFieldService _formFieldService;
//        private readonly ISubmissionService _submissionService;

//        public FormsController(IFormService formService, IFormFieldService formFieldService, ISubmissionService submissionService)
//        {
//            _formService = formService;
//            _formFieldService = formFieldService;
//            _submissionService = submissionService;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var forms = await _formService.GetAllFormsAsync();
//            return View(forms);
//        }

//        public IActionResult Create() => View();

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Name,Description,Slug,IsActive")] Form form)
//        {
//            if (ModelState.IsValid)
//            {
//                var newForm = await _formService.CreateFormAsync(form);
//                TempData["SuccessMessage"] = "فرم با موفقیت ایجاد شد.";
//                return RedirectToAction(nameof(Design), new { id = newForm.Id });
//            }
//            return View(form);
//        }

//        public async Task<IActionResult> Design(int? id)
//        {
//            if (id == null) return NotFound();
//            var form = await _formService.GetFormByIdWithFieldsAsync(id.Value);
//            if (form == null) return NotFound();
//            return View(form);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Submissions(int id)
//        {
//            var form = await _formService.GetFormWithSubmissionsAsync(id);
//            if (form == null) return NotFound();
//            return View(form);
//        }

//        [HttpGet]
//        public async Task<IActionResult> SubmissionDetails(int id)
//        {
//            var submission = await _submissionService.GetSubmissionByIdAsync(id);
//            if (submission == null) return NotFound();
//            return View(submission);
//        }

//        [HttpGet]
//        public async Task<IActionResult> ExportToCsv(int id)
//        {
//            var submissions = await _submissionService.GetSubmissionsByFormIdAsync(id);
//            if (!submissions.Any())
//            {
//                TempData["ErrorMessage"] = "هیچ داده‌ای برای خروجی گرفتن وجود ندارد.";
//                return RedirectToAction(nameof(Submissions), new { id });
//            }

//            var form = await _formService.GetFormByIdWithFieldsAsync(id);
//            if (form == null) return NotFound();

//            var builder = new StringBuilder();
//            var headers = form.FormFields.OrderBy(f => f.Order).Select(f => f.Label).ToList();
//            headers.Insert(0, "تاریخ ارسال");
//            headers.Insert(1, "آدرس IP");
//            builder.AppendLine(string.Join(",", headers));

//            foreach (var sub in submissions)
//            {
//                var rowData = new List<string>
//                {
//                    sub.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
//                    sub.IpAddress ?? "N/A"
//                };
//                foreach (var field in form.FormFields.OrderBy(f => f.Order))
//                {
//                    var value = sub.SubmissionData.FirstOrDefault(d => d.FieldName == field.Name)?.FieldValue ?? "";
//                    rowData.Add($"\"{value.Replace("\"", "\"\"")}\"");
//                }
//                builder.AppendLine(string.Join(",", rowData));
//            }

//            var fileName = $"{form.Slug}-{DateTime.UtcNow:yyyyMMdd-HHmm}.csv";
//            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", fileName);
//        }

//        // --- API Actions for Form Designer ---
//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddFieldToForm(int formId, string fieldType)
//        {
//            var newField = await _formFieldService.AddFieldToFormAsync(formId, fieldType);
//            return newField != null ? PartialView("_FormFieldItem", newField) : BadRequest();
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateFieldOrder([FromBody] UpdateOrderViewModel model)
//        {
//            await _formFieldService.UpdateFieldOrderAsync(model.FormId, model.FieldIds);
//            return Ok();
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetField(int id)
//        {
//            var field = await _formFieldService.GetFieldByIdAsync(id);
//            return field != null ? Ok(field) : NotFound();
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateField([FromBody] FormField fieldData)
//        {
//            var updatedField = await _formFieldService.UpdateFieldAsync(fieldData);
//            return updatedField != null ? PartialView("_FormFieldItem", updatedField) : NotFound();
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteField(int id)
//        {
//            var success = await _formFieldService.DeleteFieldAsync(id);
//            return success ? Ok() : NotFound();
//        }
//    }
//}

