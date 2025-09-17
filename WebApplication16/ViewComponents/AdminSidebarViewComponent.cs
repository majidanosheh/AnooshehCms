using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;


namespace WebApplication16.ViewComponents
{
    public class AdminSidebarViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public AdminSidebarViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // تمام منطق پیچیده حالا در سرویس قرار دارد
            var menuItems = await _menuService.GetAdminSidebarAsync(UserClaimsPrincipal);
            return View(menuItems);
        }
    }
}