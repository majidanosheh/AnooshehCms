using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Enums;
using WebApplication16.ViewModels;
using WebApplication16.Models;


namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // در آینده با Policy امن شود
    public class ContactSubmissionsController : Controller
    {
        private readonly WebApplication16Context _context;

        public ContactSubmissionsController(WebApplication16Context context)
        {
            _context = context;
        }

        // GET: /Admin/ContactSubmissions
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var submissionsQuery = _context.contactSubmissions.OrderByDescending(s => s.CreatedAt);
            int pageSize = 10;
            var paginatedList = await PaginatedList<ContactSubmission>.CreateAsync(submissionsQuery.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(paginatedList);
        }

        // GET: /Admin/ContactSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var submission = await _context.contactSubmissions.FirstOrDefaultAsync(m => m.Id == id);
            if (submission == null) return NotFound();

            // تغییر وضعیت به "خوانده شده"
            if (submission.Status == SubmissionStatus.New)
            {
                submission.Status = SubmissionStatus.Read;
                await _context.SaveChangesAsync();
            }

            return View(submission);
        }

        // POST: /Admin/ContactSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submission = await _context.contactSubmissions.FindAsync(id);
            if (submission != null)
            {
                _context.contactSubmissions.Remove(submission);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "پیام با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }
    }
}