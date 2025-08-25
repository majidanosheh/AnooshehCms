using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;

namespace WebApplication16.ViewComponents
{
    public class AdminSidebarViewComponent : ViewComponent
    {
        private readonly WebApplication16Context _context;

        public AdminSidebarViewComponent(WebApplication16Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // ما به دنبال منویی با نام مشخص "AdminSidebarMenu" می‌گردیم
            var menu = await _context.Menus
                .Include(m => m.MenuItems)
                    .ThenInclude(mi => mi.SubMenuItems)
                .FirstOrDefaultAsync(m => m.Name == "AdminSidebarMenu");

            if (menu == null)
            {
                return Content(string.Empty); // اگر منو وجود نداشت، چیزی نمایش نده
            }

            var topLevelItems = menu.MenuItems
                                    .Where(mi => mi.ParentMenuItemId == null)
                                    .OrderBy(mi => mi.Order)
                                    .ToList();

            return View(topLevelItems);
        }
    }
}