using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Enums;
using WebApplication16.Extensions;
using WebApplication16.Models;
using WebApplication16.ViewModels;
using YourCmsName.ViewModels;


namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly WebApplication16Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketsController(WebApplication16Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //public async Task<IActionResult> Index(int? pageNumber)
        //{
        //    var ticketsQuery = _context.Tickets.AsNoTracking();

        //    var viewModel = new TicketIndexViewModel
        //    {
        //        TotalTickets = await ticketsQuery.CountAsync(),
        //        OpenTickets = await ticketsQuery.CountAsync(t => t.Status == TicketStatus.Open || t.Status == TicketStatus.InProgress),
        //        ClosedTickets = await ticketsQuery.CountAsync(t => t.Status == TicketStatus.Closed),
        //        RespondedTickets = await ticketsQuery.CountAsync(t => t.Status == TicketStatus.Answered)
        //    };

        //    var pagedQuery = ticketsQuery
        //        .Include(t => t.User)
        //        .Include(t => t.AssignedToUser)
        //        .Select(t => new TicketViewModel
        //        {
        //            Id = t.Id,
        //            Title = t.Title,
        //            Status = t.Status,
        //            Priority = t.Priority,
        //            UserEmail = t.User.Email,
        //            AssignedToUserEmail = t.AssignedToUser != null ? t.AssignedToUser.Email : "",
        //            CreatedAt = t.CreatedAt
        //        }).OrderByDescending(t => t.CreatedAt);

        //    int pageSize = 10;
        //    viewModel.PaginatedTickets = await PaginatedList<TicketViewModel>.CreateAsync(pagedQuery, pageNumber ?? 1, pageSize);

        //    return View(viewModel);
        //}

       // GET: /Admin/Tickets
        public async Task<IActionResult> Index(int? pageNumber)
        {
            // ما از همان ViewModel قبلی استفاده می‌کنیم
            var ticketsQuery = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.AssignedToUser)
                .Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    Priority = t.Priority,
                    UserEmail = t.User.Email,
                    AssignedToUserEmail = t.AssignedToUser != null ? t.AssignedToUser.Email : "",
                    CreatedAt = t.CreatedAt
                }).OrderByDescending(t => t.CreatedAt);

            int pageSize = 10; // تعداد آیتم‌ها در هر صفحه
            var paginatedTickets = await PaginatedList<TicketViewModel>.CreateAsync(ticketsQuery.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(paginatedTickets);
        }

        // POST: /Admin/Tickets/LoadData (نسخه کامل و اصلاح شده)
        //[HttpPost]
        //public async Task<IActionResult> LoadData()
        //{
        //    var draw = Request.Form["draw"].FirstOrDefault();
        //    var start = Request.Form["start"].FirstOrDefault();
        //    var length = Request.Form["length"].FirstOrDefault();
        //    var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //    var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //    var searchValue = Request.Form["search[value]"].FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;

        //    var ticketsQuery = _context.Tickets
        //        .Include(t => t.User)
        //        .Include(t => t.AssignedToUser)
        //        .Select(t => new TicketViewModel
        //        {
        //            Id = t.Id,
        //            Title = t.Title,
        //            Status = t.Status,
        //            Priority = t.Priority,
        //            UserEmail = t.User.Email,
        //            AssignedToUserEmail = t.AssignedToUser != null ? t.AssignedToUser.Email : "",
        //            CreatedAt = t.CreatedAt
        //        });

        //    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        //    {
        //        ticketsQuery = ticketsQuery.OrderBy(sortColumn + " " + sortColumnDirection);
        //    }

        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        ticketsQuery = ticketsQuery.Where(m => m.Title.Contains(searchValue) || m.UserEmail.Contains(searchValue));
        //    }

        //    int recordsTotal = await ticketsQuery.CountAsync();
        //    var data = await ticketsQuery.Skip(skip).Take(pageSize).ToListAsync();
        //    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

        //    return Ok(jsonData);
        //}

        // GET: Admin/Tickets/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new TicketAdminViewModel();
            await PopulateAdminDropdownsAsync(viewModel);
            return View(viewModel);
        }

        // POST: Admin/Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var ticket = new Ticket
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Status = viewModel.Status,
                    Priority = viewModel.Priority,
                    UserId = viewModel.UserId,
                    AssignedToUserId = viewModel.AssignedToUserId
                };
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تیکت با موفقیت ایجاد شد.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateAdminDropdownsAsync(viewModel);
            return View(viewModel);
        }

        // GET: Admin/Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            var viewModel = new TicketAdminViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                UserId = ticket.UserId,
                AssignedToUserId = ticket.AssignedToUserId
            };

            await PopulateAdminDropdownsAsync(viewModel);
            return View(viewModel);
        }

        // POST: Admin/Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketAdminViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var ticketToUpdate = await _context.Tickets.FindAsync(id);
                if (ticketToUpdate == null) return NotFound();

                ticketToUpdate.Title = viewModel.Title;
                ticketToUpdate.Description = viewModel.Description;
                ticketToUpdate.Status = viewModel.Status;
                ticketToUpdate.Priority = viewModel.Priority;
                ticketToUpdate.UserId = viewModel.UserId;
                ticketToUpdate.AssignedToUserId = viewModel.AssignedToUserId;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تیکت با موفقیت ویرایش شد.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateAdminDropdownsAsync(viewModel);
            return View(viewModel);
        }

        // GET: Admin/Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var ticket = await _context.Tickets
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        // POST: Admin/Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تیکت با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAdminDropdownsAsync(TicketAdminViewModel model)
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            model.Users = new SelectList(users, "Id", "Email", model.UserId);
            model.Statuses = new SelectList(Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().Select(v => new { Id = (int)v, Name = v.GetDisplayName() }), "Id", "Name", model.Status);
            model.Priorities = new SelectList(Enum.GetValues(typeof(TicketPriority)).Cast<TicketPriority>().Select(v => new { Id = (int)v, Name = v.GetDisplayName() }), "Id", "Name", model.Priority);
        }
    }
}
