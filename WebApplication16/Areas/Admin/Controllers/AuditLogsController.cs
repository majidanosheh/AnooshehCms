using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Constants;
using WebApplication16.ViewModels;


namespace WebApplication16.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Permissions.AuditLogs.View)]
    public class AuditLogsController : Controller
    {
        private readonly WebApplication16Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AuditLogsController(WebApplication16Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 20;
            var logsQuery = _context.AuditLogs.AsQueryable();

            var userIds = await logsQuery.Select(l => l.UserId).Distinct().ToListAsync();
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Email);

            var logs = await logsQuery
                .OrderByDescending(l => l.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(log => new AuditLogViewModel
                {
                    UserEmail = users.ContainsKey(log.UserId) ? users[log.UserId] : "کاربر حذف شده/سیستم",
                    ActionType = log.ActionType.ToString(),
                    EntityName = log.EntityName,
                    Details = log.Details,
                    Timestamp = log.Timestamp
                })
                .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling(await logsQuery.CountAsync() / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(logs);
        }
    }
}