using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.Services;
using WebApplication16.ViewModels;



namespace WebApplication16.Services
{
    public class MenuService : IMenuService
    {
        private readonly WebApplication16Context _context;
        private readonly IAuthorizationService _authorizationService;

        public MenuService(WebApplication16Context context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public async Task<List<MenuItemViewModel>> GetAdminSidebarAsync(ClaimsPrincipal user)
        {
            // ۱. تمام آیتم‌های منو را در یک کوئری بهینه به صورت یک لیست صاف بارگذاری می‌کنیم
            var allItems = await _context.MenuItems
                .Where(mi => mi.Menu.Name == "AdminSidebarMenu")
                .OrderBy(mi => mi.Order)
                .AsNoTracking()
                .ToListAsync();

            if (!allItems.Any()) return new List<MenuItemViewModel>();

            // ۲. تمام آیتم‌ها را بر اساس مجوزهای کاربر فیلتر می‌کنیم
            var accessibleItems = new List<MenuItem>();
            foreach (var item in allItems)
            {
                if (string.IsNullOrEmpty(item.RequiredPermission) || (await _authorizationService.AuthorizeAsync(user, item.RequiredPermission)).Succeeded)
                {
                    accessibleItems.Add(item);
                }
            }

            // ۳. حالا ساختار درختی را در حافظه می‌سازیم
            var lookup = accessibleItems.ToLookup(item => item.ParentMenuItemId);
            var topLevelItems = lookup[null]; // آیتم‌هایی که والد ندارند

            return topLevelItems.Select(item => BuildViewModel(item, lookup)).ToList();
        }

        // این متد بازگشتی، ViewModel را از روی ساختار درختی که در حافظه ساختیم، ایجاد می‌کند
        private MenuItemViewModel BuildViewModel(MenuItem item, ILookup<int?, MenuItem> lookup)
        {
            var viewModel = new MenuItemViewModel
            {
                Title = item.Title,
                Url = item.Url,
                IconClass = item.IconClass,
                // به صورت بازگشتی، فرزندان این آیتم را پیدا کرده و برای آن‌ها نیز ViewModel می‌سازیم
                SubItems = lookup[item.Id].Select(subItem => BuildViewModel(subItem, lookup)).ToList()
            };

            return viewModel;
        }
    }
}
