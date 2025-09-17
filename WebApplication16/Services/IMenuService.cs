using System.Security.Claims;
using WebApplication16.ViewModels;

namespace WebApplication16.Services
{
    public interface IMenuService
    {
        Task<List<MenuItemViewModel>> GetAdminSidebarAsync(ClaimsPrincipal user);
    }
}
