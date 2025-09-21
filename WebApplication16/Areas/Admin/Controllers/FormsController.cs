using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication16.Enums;
using WebApplication16.Models;
using WebApplication16.Services;

namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FormsController : Controller
    {
        private readonly IFormService _formService;
        private readonly IFormFieldService _formFieldService;

        public FormsController(IFormService formService, IFormFieldService formFieldService)
        {
            _formService = formService;
            _formFieldService = formFieldService;
        }

        public async Task<IActionResult> Index()
        {
            var forms = await _formService.GetAllFormsAsync();
            return View(forms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Slug,IsActive")] Form form)
        {
            if (ModelState.IsValid)
            {
                var createdForm = await _formService.CreateFormAsync(form);
                TempData["SuccessMessage"] = "فرم با موفقیت ایجاد شد.";
                return RedirectToAction(nameof(Design), new { id = createdForm.Id });
            }
            return View(form);
        }

        public async Task<IActionResult> Design(int id)
        {
            var form = await _formService.GetFormWithFieldsAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFieldToForm(int formId, FieldType fieldType)
        {
            var newField = await _formFieldService.AddFieldToFormAsync(formId, fieldType);
            return PartialView("_FormFieldItem", newField);
        }

        // POST: /Admin/Forms/UpdateFieldOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        // This action is corrected to accept FormData
        public async Task<IActionResult> UpdateFieldOrder(int formId, List<int> fieldIds)
        {
            if (fieldIds == null || !fieldIds.Any())
            {
                return BadRequest("لیست شناسه‌ها نمی‌تواند خالی باشد.");
            }

            await _formFieldService.UpdateFieldOrderAsync(formId, fieldIds);
            return Ok(new { success = true, message = "ترتیب فیلدها با موفقیت ذخیره شد." });
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
        public async Task<IActionResult> UpdateField(int id, [FromBody] FormField updatedField)
        {
            if (id != updatedField.Id) return BadRequest();

            var field = await _formFieldService.UpdateFieldAsync(id, updatedField.Label, updatedField.IsRequired);

            return PartialView("_FormFieldItem", field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // This action is corrected to read the ID from the request body
        public async Task<IActionResult> DeleteField(int id)
        {
            await _formFieldService.DeleteFieldAsync(id);
            return Ok(new { success = true, message = "فیلد با موفقیت حذف شد." });
        }
    }
}

