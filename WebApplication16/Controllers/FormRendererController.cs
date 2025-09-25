using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;

namespace WebApplication16.Controllers
{
    public class FormRendererController : Controller
    {
        private readonly IFormService _formService;
        private readonly ISubmissionService _submissionService;

        public FormRendererController(IFormService formService, ISubmissionService submissionService)
        {
            _formService = formService;
            _submissionService = submissionService;
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

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _submissionService.CreateSubmissionAsync(form.Id, Request.Form, ipAddress);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "فرم شما با موفقیت ارسال شد.";
                return RedirectToAction("Display", new { slug });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View("Display", form);
        }
    }
}



//using Microsoft.AspNetCore.Mvc;
//using WebApplication16.Services;

//namespace WebApplication16.Controllers
//{
//    public class FormRendererController : Controller
//    {
//        private readonly IFormService _formService;
//        private readonly ISubmissionService _submissionService;

//        public FormRendererController(IFormService formService, ISubmissionService submissionService)
//        {
//            _formService = formService;
//            _submissionService = submissionService;
//        }

//        [HttpGet("form/{slug}")]
//        public async Task<IActionResult> Display(string slug)
//        {
//            var form = await _formService.GetFormBySlugWithFieldsAsync(slug);
//            if (form == null || !form.IsActive)
//            {
//                return NotFound();
//            }
//            return View(form);
//        }

//        [HttpPost("form/{slug}")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Submit(string slug)
//        {
//            var form = await _formService.GetFormBySlugWithFieldsAsync(slug);
//            if (form == null)
//            {
//                return NotFound();
//            }

//            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//            var result = await _submissionService.CreateSubmissionAsync(form.Id, Request.Form, ipAddress);

//            if (result.Success)
//            {
//                TempData["SuccessMessage"] = "فرم شما با موفقیت ارسال شد.";
//                return RedirectToAction("Display", new { slug });
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error);
//            }

//            return View("Display", form);
//        }
//    }
//}



//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using WebApplication16.Services;


//namespace WebApplication16.Controllers
//{
//    public class FormRendererController : Controller
//    {
//        private readonly IFormService _formService;
//        private readonly ISubmissionService _submissionService;

//        public FormRendererController(IFormService formService, ISubmissionService submissionService)
//        {
//            _formService = formService;
//            _submissionService = submissionService;
//        }

//        [HttpGet("form/{slug}")]
//        public async Task<IActionResult> Display(string slug)
//        {
//            var form = await _formService.GetFormBySlugAsync(slug);
//            if (form == null)
//            {
//                return NotFound();
//            }
//            return View(form);
//        }

//        [HttpPost("form/{slug}")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Submit(string slug)
//        {
//            var form = await _formService.GetFormBySlugAsync(slug);
//            if (form == null)
//            {
//                return NotFound();
//            }

//            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//            var success = await _submissionService.CreateSubmissionAsync(form.Id, Request.Form, ipAddress);

//            if (success)
//            {
//                TempData["SuccessMessage"] = "فرم شما با موفقیت ارسال شد. متشکریم!";
//            }
//            else
//            {
//                TempData["ErrorMessage"] = "خطایی در ارسال فرم رخ داد. لطفا دوباره تلاش کنید.";
//            }

//            return RedirectToAction("Display", new { slug = slug });
//        }
//    }
//}
