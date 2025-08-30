using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.ViewModels;


namespace WebApplication16.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly WebApplication16Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(WebApplication16Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // توضیح بهینه‌سازی: ما از متدهای Async (مانند CountAsync) استفاده می‌کنیم.
            // این کار باعث می‌شود که در زمان اجرای کوئری‌های پایگاه داده، نخ اصلی برنامه آزاد شده
            // و بتواند به درخواست‌های دیگر پاسخ دهد. این کار مقیاس‌پذیری برنامه را به شدت افزایش می‌دهد.
            var pagesCount = await _context.Pages.CountAsync();
            var postsCount = await _context.Posts.CountAsync();

            var viewModel = new DashboardViewModel
            {
                PagesCount = pagesCount,
                PostsCount = postsCount,
                UsersCount = await _userManager.Users.CountAsync(),
                MenusCount = await _context.Menus.CountAsync(),
                DoughnutChartData = new List<int> { pagesCount, postsCount }
            };

            // --- آماده‌سازی داده‌ها برای نمودار میله‌ای "مقالات ایجاد شده در ۷ روز گذشته" ---
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            // توضیح کوئری LINQ:
            // .GroupBy(p => p.CreatedAt.Date): مقالات را بر اساس روز ایجادشان گروه‌بندی می‌کند.
            // .Select(g => new { ... }): برای هر گروه (هر روز)، یک شیء جدید می‌سازد که شامل تاریخ و تعداد مقالات آن روز است.
            var postsData = await _context.Posts
                .Where(p => p.CreatedAt >= sevenDaysAgo)
                .GroupBy(p => p.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // این حلقه تضمین می‌کند که اگر در یک روز خاص هیچ مقاله‌ای ایجاد نشده باشد،
            // مقدار صفر برای آن روز در نمودار نمایش داده شود.
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.UtcNow.AddDays(-i).Date;
                var dataPoint = postsData.FirstOrDefault(d => d.Date == date);
                viewModel.BarChartLabels.Add(date.ToString("yyyy-MM-dd"));
                viewModel.BarChartData.Add(dataPoint?.Count ?? 0);
            }

            // --- آماده‌سازی داده‌ها برای ویجت "آخرین فعالیت‌ها" ---
            // این یک کوئری بهینه برای جلوگیری از مشکل N+1 است.
            var userIds = await _context.AuditLogs.Select(l => l.UserId).Distinct().Take(5).ToListAsync();
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Email);

            viewModel.RecentActivities = await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(5) // فقط ۵ فعالیت آخر را می‌خوانیم
                .Select(a => new AuditLogViewModel
                {
                    UserEmail = users.ContainsKey(a.UserId) ? users[a.UserId] : "سیستم",
                    ActionType = a.ActionType.ToString(),
                    EntityName = a.EntityName,
                    Timestamp = a.Timestamp
                }).ToListAsync();

            return View(viewModel);
        }
    }
}