using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;

namespace YourCmsName.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AllowedFileTypesController : Controller
    {
        private readonly WebApplication16Context _context;

        public AllowedFileTypesController(WebApplication16Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var fileTypeData = _context.allowedFileTypes.AsNoTracking();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                fileTypeData = fileTypeData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                fileTypeData = fileTypeData.Where(m => m.Extension.Contains(searchValue) || m.MimeType.Contains(searchValue));
            }

            recordsTotal = await fileTypeData.CountAsync();
            var data = await fileTypeData.Skip(skip).Take(pageSize).ToListAsync();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

            return Ok(jsonData);
        }

        // GET: Admin/AllowedFileTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AllowedFileTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AllowedFileType allowedFileType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allowedFileType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allowedFileType);
        }

        // GET: Admin/AllowedFileTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var allowedFileType = await _context.allowedFileTypes.FindAsync(id);
            if (allowedFileType == null) return NotFound();
            return View(allowedFileType);
        }

        // POST: Admin/AllowedFileTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AllowedFileType allowedFileType)
        {
            if (id != allowedFileType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allowedFileType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.allowedFileTypes.AnyAsync(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(allowedFileType);
        }

        // GET: Admin/AllowedFileTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var allowedFileType = await _context.allowedFileTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (allowedFileType == null) return NotFound();
            return View(allowedFileType);
        }

        // POST: Admin/AllowedFileTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allowedFileType = await _context.allowedFileTypes.FindAsync(id);
            if (allowedFileType != null)
            {
                _context.allowedFileTypes.Remove(allowedFileType);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}