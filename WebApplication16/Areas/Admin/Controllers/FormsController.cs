using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication16.Services; // Using the new Service Layer
using WebApplication16.Models;
using WebApplication16.ViewModels; // Added for the new ViewModel
using Microsoft.EntityFrameworkCore;


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

        // GET: /Admin/Forms/Design/5
        public async Task<IActionResult> Design(int? id)
        {
            if (id == null) return NotFound();
            var form = await _formService.GetFormWithFieldsAsync(id.Value);
            if (form == null) return NotFound();
            return View(form);
        }

        // POST: /Admin/Forms/AddFieldToForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFieldToForm(int formId, string fieldType)
        {
            var newField = await _formFieldService.AddFieldToFormAsync(formId, fieldType);
            if (newField == null) return BadRequest("نوع فیلد نامعتبر است.");

            return PartialView("_FormFieldItem", newField);
        }

        // POST: /Admin/Forms/UpdateFieldOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFieldOrder([FromBody] UpdateOrderViewModel model)
        {
            if (model == null || model.FieldIds == null)
            {
                return BadRequest("داده‌های نامعتبر.");
            }

            await _formFieldService.UpdateFieldOrderAsync(model.FormId, model.FieldIds);
            return Ok(new { success = true, message = "ترتیب فیلدها با موفقیت ذخیره شد." });
        }


        // GET: /Admin/Forms/GetField/5
        [HttpGet]
        public async Task<IActionResult> GetField(int id)
        {
            var field = await _formFieldService.GetFieldByIdAsync(id);
            if (field == null) return NotFound();
            return Ok(field);
        }

        // POST: /Admin/Forms/UpdateField/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateField(int id, [FromBody] FormField updatedField)
        {
            if (id != updatedField.Id) return BadRequest();

            var field = await _formFieldService.UpdateFieldAsync(updatedField);
            if (field == null) return NotFound();

            return PartialView("_FormFieldItem", field);
        }

        // POST: /Admin/Forms/DeleteField/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteField(int id)
        {
            await _formFieldService.DeleteFieldAsync(id);
            return Ok(new { success = true, message = "فیلد با موفقیت حذف شد." });
        }

        [HttpGet]
        public async Task<IActionResult> Submissions(int id)
        {
            var form = await _formService.GetFormWithSubmissionsAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return View(form);
        }
    }
}

