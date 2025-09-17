using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.Services;
using WebApplication16.ViewModels;


namespace WebApplication16.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationService _notificationService;
        private readonly WebApplication16Context _context;

        // سازنده (Constructor) کامل که تمام سرویس‌های لازم را دریافت می‌کند
        public HomeController(ILogger<HomeController> logger, INotificationService notificationService, WebApplication16Context context)
        {
            _logger = logger;
            _notificationService = notificationService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // POST: /Home/Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ۱. ذخیره پیام در پایگاه داده
                var submission = new ContactSubmission
                {
                    Name = model.Name,
                    Email = model.Email,
                    Subject = model.Subject,
                    Message = model.Message
                };
                _context.Add(submission);
                await _context.SaveChangesAsync();

                // ۲. ارسال ایمیل
                var adminEmail = "admin@yourcms.com";
                var subject = $"پیام جدید از فرم تماس (ID: {submission.Id}): {model.Subject}";
                var messageBody = $"<p>شما یک پیام جدید از طرف <strong>{model.Name}</strong> با ایمیل {model.Email} دریافت کرده‌اید.</p>" +
                                  $"<hr/><p><strong>متن پیام:</strong></p><p>{model.Message}</p>";

                try
                {
                    await _notificationService.SendEmailAsync(adminEmail, subject, messageBody);
                    TempData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد.";
                    return RedirectToAction(nameof(Contact));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "خطا در ارسال ایمیل فرم تماس");
                    ModelState.AddModelError(string.Empty, "متاسفانه در حال حاضر امکان ارسال پیام وجود ندارد.");
                }
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new WebApplication16.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}