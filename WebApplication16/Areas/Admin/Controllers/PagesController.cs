using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication16.Areas.Identity.DataAccess;
using YourCmsName.Models;

namespace WebApplication16.Areas.Admin.Controllers
{
    //[Authorize]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly WebApplication16Context _context;

        public PagesController(WebApplication16Context context)
        {
            _context = context;
        }

        // GET: Admin/Pages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pages.ToListAsync());
        }

        // GET: Admin/Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Admin/Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Slug,Id,CreatedAt,ModifiedAt,CreatedBy,ModifiedBy")] Page page)
        {
            if (ModelState.IsValid)
            {
                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(page);
        }

        // GET: Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Content,Slug,Id,CreatedAt,ModifiedAt,CreatedBy,ModifiedBy")] Page page)
        {
            if (id != page.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.Id))
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
            return View(page);
        }

        // GET: Admin/Pages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                _context.Pages.Remove(page);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.Id == id);
        }
    }
}
