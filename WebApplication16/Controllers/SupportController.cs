using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.ViewModels;


namespace WebApplication16.Controllers
{
    [Authorize] // فقط کاربران لاگین کرده به این کنترلر دسترسی دارند
    public class SupportController : Controller
    {
        private readonly WebApplication16Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SupportController(WebApplication16Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Support (لیست تیکت‌های من)
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var userTickets = await _context.Tickets
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(userTickets);
        }

        // GET: /Support/Create
        public IActionResult Create()
        {
            // ما یک نمونه خالی از ViewModel جدید را به ویو ارسال می‌کنیم
            return View(new TicketCreateViewModel());
        }

        // POST: /Support/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCreateViewModel viewModel) // <<-- ورودی به ViewModel تغییر کرد
        {
            if (ModelState.IsValid)
            {
                // نگاشت دستی از ViewModel به Entity
                var ticket = new Ticket
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Priority = viewModel.Priority,
                    UserId = _userManager.GetUserId(User) // UserId را از کاربر لاگین کرده می‌خوانیم
                };

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تیکت شما با موفقیت ثبت شد.";
                return RedirectToAction(nameof(Index));
            }
            // اگر مدل معتبر نبود، همان ViewModel را به ویو برمی‌گردانیم
            return View(viewModel);
        }

        // GET: /Support/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var ticket = await _context.Tickets
                .Include(t => t.Replies)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId); // بررسی امنیتی: کاربر فقط به تیکت‌های خودش دسترسی دارد

            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // POST: /Support/AddReply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReply(int ticketId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["ErrorMessage"] = "متن پاسخ نمی‌تواند خالی باشد.";
                return RedirectToAction(nameof(Details), new { id = ticketId });
            }

            var userId = _userManager.GetUserId(User);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.UserId == userId);

            if (ticket == null) return NotFound();

            var reply = new TicketReply
            {
                Message = message,
                TicketId = ticketId,
                UserId = userId
            };

            _context.TicketReplies.Add(reply);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "پاسخ شما با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = ticketId });
        }
    }
}
