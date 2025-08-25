using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.ViewModels;

namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenusController : Controller
    {
        private readonly WebApplication16Context _context;

        public MenusController(WebApplication16Context context)
        {
            _context = context;
        }

        // GET: Admin/Menus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Menus.ToListAsync());
        }

        // GET: Admin/Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // توضیح معماری: ما از Include و ThenInclude برای بارگذاری حریصانه (Eager Loading) روابط تو در تو استفاده می‌کنیم.
            // این کار به Entity Framework می‌گوید که تمام اطلاعات مورد نیاز (منو، آیتم‌های سطح اول، و زیرآیتم‌های آنها) را
            // تنها با یک کوئری به پایگاه داده، بارگذاری کند. این روش از مشکل معروف "N+1 Query" جلوگیری می‌کند
            // که در آن برای هر آیتم، یک کوئری جداگانه به دیتابیس زده می‌شود و عملکرد را به شدت کاهش می‌دهد.
            var menu = await _context.Menus
                .Include(m => m.MenuItems)
                    .ThenInclude(mi => mi.SubMenuItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            // ما فقط آیتم‌های سطح بالا (آنهایی که والد ندارند) را به ویو ارسال می‌کنیم.
            // زیرآیتم‌های هر آیتم، از طریق ویژگی ناوبری SubMenuItems که قبلاً بارگذاری کرده‌ایم، در خود ویو قابل دسترسی خواهند بود.
            ViewBag.MenuItems = menu.MenuItems.Where(mi => mi.ParentMenuItemId == null).OrderBy(mi => mi.Order).ToList();

            return View(menu);
        }

        // GET: Admin/Menus/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }
        private async Task PopulateDropdownsAsync(int menuId)
        {
            ViewBag.PossibleParents = await _context.MenuItems
                .Where(mi => mi.MenuId == menuId)
                .Select(mi => new SelectListItem { Value = mi.Id.ToString(), Text = mi.Title })
                .ToListAsync();

            ViewBag.InternalPages = await _context.Pages
                .Select(p => new SelectListItem { Value = "/" + p.Slug, Text = p.Title })
                .ToListAsync();
        }

       
        // GET: Admin/Menus/CreateItem
        public async Task<IActionResult> CreateItem(int menuId)
        {
            var viewModel = new MenuItemViewModel { MenuId = menuId };
            await PopulateDropdownsAsync(menuId);
            return View("MenuItemForm", viewModel);
        }

        // POST: Admin/Menus/CreateItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(MenuItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var menuItem = new MenuItem
                {
                    Title = viewModel.Title,
                    Url = viewModel.Url,
                    Order = viewModel.Order,
                    MenuId = viewModel.MenuId,
                    ParentMenuItemId = viewModel.ParentMenuItemId,
                    OpenInNewTab = viewModel.OpenInNewTab
                };

                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = viewModel.MenuId });
            }

            await PopulateDropdownsAsync(viewModel.MenuId);
            return View("MenuItemForm", viewModel);
        }

        private void PopulateDropdowns(int menuId)
        {
            ViewBag.PossibleParents = _context.MenuItems
                .Where(mi => mi.MenuId == menuId)
                .Select(mi => new SelectListItem { Value = mi.Id.ToString(), Text = mi.Title })
                .ToList();

            ViewBag.InternalPages = _context.Pages
                .Select(p => new SelectListItem { Value = "/" + p.Slug, Text = p.Title })
                .ToList();
        }

        // GET: Admin/Menus/EditItem/5
        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();

            var viewModel = new MenuItemViewModel
            {
                Id = menuItem.Id,
                Title = menuItem.Title,
                Url = menuItem.Url,
                Order = menuItem.Order,
                MenuId = menuItem.MenuId,
                ParentMenuItemId = menuItem.ParentMenuItemId,
                OpenInNewTab = menuItem.OpenInNewTab
            };

            await PopulateDropdownsAsync(viewModel.MenuId);
            return View("MenuItemForm", viewModel);
        }

        // POST: Admin/Menus/EditItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, MenuItemViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var menuItemToUpdate = await _context.MenuItems.FindAsync(id);
                if (menuItemToUpdate == null) return NotFound();

                menuItemToUpdate.Title = viewModel.Title;
                menuItemToUpdate.Url = viewModel.Url;
                menuItemToUpdate.Order = viewModel.Order;
                menuItemToUpdate.ParentMenuItemId = viewModel.ParentMenuItemId;
                menuItemToUpdate.OpenInNewTab = viewModel.OpenInNewTab;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = viewModel.MenuId });
            }

            await PopulateDropdownsAsync(viewModel.MenuId);
            return View("MenuItemForm", viewModel);
        }

        // GET: Admin/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Admin/Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Menus/DeleteItem/5
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
                .Include(m => m.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null) return NotFound();

            return View("MenuItemDelete", menuItem);
        }

        // POST: Admin/Menus/DeleteItem/5
        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItemConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = menuItem.MenuId });
            }
            return RedirectToAction(nameof(Index));
        }
        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }




    }
}
