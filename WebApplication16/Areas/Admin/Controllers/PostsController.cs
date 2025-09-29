using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;
using WebApplication16.Models;
using WebApplication16.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // برای IFormFile

namespace WebApplication16.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        // ... سایر سرویس ها

        public PostsController(IFileStorageService fileStorageService /*, ...*/)
        {
            _fileStorageService = fileStorageService;
            // ...
        }

        // POST: /Admin/Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    // ... سایر پراپرتی ها
                };

                // **اصلاح نهایی:** ذخیره تصویر شاخص با استفاده از Stream
                if (model.ThumbnailFile != null)
                {
                    using (var stream = model.ThumbnailFile.OpenReadStream())
                    {
                        post.ThumbnailUrl = await _fileStorageService.StoreFileAsync(stream, model.ThumbnailFile.FileName, "uploads/posts");
                    }
                }

                // await _postService.CreatePostAsync(post);
                TempData["SuccessMessage"] = "پست با موفقیت ایجاد شد.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: /Admin/Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // var existingPost = await _postService.GetPostByIdAsync(id);

                // **اصلاح نهایی:** ذخیره تصویر شاخص با استفاده از Stream
                if (model.ThumbnailFile != null)
                {
                    using (var stream = model.ThumbnailFile.OpenReadStream())
                    {
                        // حذف فایل قبلی در صورت نیاز
                        // if (!string.IsNullOrEmpty(existingPost.ThumbnailUrl))
                        // {
                        //     _fileStorageService.DeleteFile(existingPost.ThumbnailUrl);
                        // }
                        model.ThumbnailUrl = await _fileStorageService.StoreFileAsync(stream, model.ThumbnailFile.FileName, "uploads/posts");
                    }
                }

                // ... آپدیت پست

                TempData["SuccessMessage"] = "پست با موفقیت ویرایش شد.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Index() { return View(); }
        public IActionResult Details() { return View(); }
        public IActionResult Delete() { return View(); }
        // ...
    }
}


//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebApplication16.Areas.Identity.DataAccess;
//using WebApplication16.Constants;
//using WebApplication16.Models;
//using WebApplication16.Services;
//using WebApplication16.ViewModels;

//namespace WebApplication16.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize]

//    public class PostsController : Controller
//    {
//        private readonly WebApplication16Context _context;
//        private readonly IFileStorageService _fileStorageService;

//        // اصلاح سازنده برای دریافت سرویس جدید
//        public PostsController(WebApplication16Context context, IFileStorageService fileStorageService)
//        {
//            _context = context;
//            _fileStorageService = fileStorageService;
//        }

//        // GET: Admin/Posts
//        [Authorize(Policy = Permissions.Blog.ViewPosts)]
//        public async Task<IActionResult> Index()
//        {
//            var posts = await _context.Posts.Include(p => p.Category).ToListAsync();
//            return View(posts);
//        }

//        private async Task PopulateDropdownsAsync(PostViewModel? model = null)
//        {
//            var categories = await _context.Categories.AsNoTracking().ToListAsync();
//            var tags = await _context.Tags.AsNoTracking().ToListAsync();

//            if (model == null)
//            {
//                ViewBag.Categories = new SelectList(categories, "Id", "Name");
//                ViewBag.Tags = new SelectList(tags, "Id", "Name");
//            }
//            else
//            {
//                ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
//                ViewBag.Tags = new SelectList(tags, "Id", "Name", model.SelectedTagIds);
//            }
//        }

//        // GET: Admin/Posts/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Posts
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }

//            return View(post);
//        }

//        // GET: Admin/Posts/Create
//        [Authorize(Policy = Permissions.Blog.CreatePosts)]
//        public async Task<IActionResult> Create()
//        {
//            var viewModel = new PostViewModel();
//            await PopulateDropdownsAsync(viewModel);
//            return View(viewModel);
//        }

//        // POST: Admin/Posts/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Policy = Permissions.Blog.CreatePosts)]
//        public async Task<IActionResult> Create(PostViewModel viewModel)
//        {
//            if (ModelState.IsValid)
//            {
//                var post = new Post
//                {
//                    Title = viewModel.Title,
//                    Slug = viewModel.Slug,
//                    Content = viewModel.Content,
//                    Excerpt = viewModel.Excerpt,
//                    IsPublished = viewModel.IsPublished,
//                    CategoryId = viewModel.CategoryId,
//                    PublishDate = viewModel.IsPublished ? DateTime.UtcNow : null
//                };

//                // ... (قبل از این بخش، متغیر post ساخته شده است)
//                if (viewModel.FeaturedImage != null)
//                {
//                    try
//                    {
//                        post.FeaturedImageUrl = await _fileStorageService.StoreFileAsync(
//                            viewModel.FeaturedImage.OpenReadStream(),
//                            viewModel.FeaturedImage.FileName);
//                    }
//                    catch (ArgumentException ex)
//                    {
//                        ModelState.AddModelError("FeaturedImage", ex.Message);
//                    }
//                }

//                if (!ModelState.IsValid)
//                {
//                    // اینجا viewModel را پاس می‌دهیم
//                    await PopulateDropdownsAsync(viewModel);
//                    return View("Create", viewModel);
//                }

//                _context.Add(post);
//                await _context.SaveChangesAsync();

//                // افزودن تگ‌های انتخاب شده
//                foreach (var tagId in viewModel.SelectedTagIds)
//                {
//                    _context.PostTags.Add(new PostTag { PostId = post.Id, TagId = tagId });
//                }
//                await _context.SaveChangesAsync();

//                TempData["SuccessMessage"] = "مقاله با موفقیت ایجاد شد.";
//                return RedirectToAction(nameof(Index));
//            }

//            await PopulateDropdownsAsync(viewModel);
//            return View(viewModel);
//        }


//        // GET: Admin/Posts/Edit/5
//        [Authorize(Policy = Permissions.Blog.EditPosts)]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Posts
//                .Include(p => p.PostTags) // تگ‌های فعلی را بارگذاری می‌کنیم
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (post == null)
//            {
//                return NotFound();
//            }

//            // نگاشت از مدل اصلی (Entity) به ViewModel
//            var viewModel = new PostViewModel
//            {
//                Id = post.Id,
//                Title = post.Title,
//                Slug = post.Slug,
//                Content = post.Content,
//                Excerpt = post.Excerpt,
//                IsPublished = post.IsPublished,
//                CategoryId = post.CategoryId,
//                FeaturedImageUrl = post.FeaturedImageUrl,
//                SelectedTagIds = post.PostTags.Select(pt => pt.TagId).ToList()
//            };

//            await PopulateDropdownsAsync(viewModel);
//            return View(viewModel);
//        }

//        // POST: Admin/Posts/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Policy = Permissions.Blog.EditPosts)]
//        public async Task<IActionResult> Edit(int id, PostViewModel viewModel)
//        {
//            if (id != viewModel.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                var postToUpdate = await _context.Posts
//                    .Include(p => p.PostTags)
//                    .FirstOrDefaultAsync(p => p.Id == id);

//                if (postToUpdate == null)
//                {
//                    return NotFound();
//                }

//                // به‌روزرسانی فیلدهای ساده
//                postToUpdate.Title = viewModel.Title;
//                postToUpdate.Slug = viewModel.Slug;
//                postToUpdate.Content = viewModel.Content;
//                postToUpdate.Excerpt = viewModel.Excerpt;
//                postToUpdate.IsPublished = viewModel.IsPublished;
//                postToUpdate.CategoryId = viewModel.CategoryId;
//                postToUpdate.PublishDate = viewModel.IsPublished ? DateTime.UtcNow : null;

//                // ... (قبل از این بخش، متغیر postToUpdate از دیتابیس خوانده شده است)
//                if (viewModel.FeaturedImage != null)
//                {
//                    try
//                    {
//                        // اینجا از متغیر صحیح استفاده می‌کنیم
//                        postToUpdate.FeaturedImageUrl = await _fileStorageService.StoreFileAsync(
//                            viewModel.FeaturedImage.OpenReadStream(),
//                            viewModel.FeaturedImage.FileName);
//                    }
//                    catch (ArgumentException ex)
//                    {
//                        ModelState.AddModelError("FeaturedImage", ex.Message);
//                    }
//                }

//                if (!ModelState.IsValid)
//                {
//                    // اینجا viewModel را پاس می‌دهیم
//                    await PopulateDropdownsAsync(viewModel);
//                    return View("Edit", viewModel);
//                }

//                if (!ModelState.IsValid)
//                {
//                    // اینجا viewModel را پاس می‌دهیم
//                    await PopulateDropdownsAsync(viewModel);
//                    return View("Create", viewModel);
//                }

//                // به‌روزرسانی تگ‌ها (رابطه چند-به-چند)
//                // استراتژی ساده و قابل اطمینان: حذف همه، افزودن منتخب‌ها
//                postToUpdate.PostTags.Clear();
//                foreach (var tagId in viewModel.SelectedTagIds)
//                {
//                    postToUpdate.PostTags.Add(new PostTag { TagId = tagId });
//                }

//                try
//                {
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Posts.Any(e => e.Id == postToUpdate.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }

//            await PopulateDropdownsAsync(viewModel);
//            return View(viewModel);
//        }

//        // GET: Admin/Posts/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Posts
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }

//            return View(post);
//        }

//        // POST: Admin/Posts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var post = await _context.Posts.FindAsync(id);
//            if (post != null)
//            {
//                _context.Posts.Remove(post);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool PostExists(int id)
//        {
//            return _context.Posts.Any(e => e.Id == id);
//        }
//    }
//}