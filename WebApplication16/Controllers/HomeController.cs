using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication16.Models;
using WebApplication16.ViewModels;



namespace WebApplication16.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //throw new Exception("این یک خطای آزمایشی است.");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var viewModel = new WebApplication16.ViewModels.ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = "متاسفانه در پردازش درخواست شما خطایی رخ داده است. لطفاً دوباره تلاش کنید یا با پشتیبانی تماس بگیرید."
            };
            return View(viewModel);
        }




    }
}
