using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.BlogVM;
using Pustok0.Context;
using Pustok0.Models;
using Pustok0.ViewModels;
using System.Runtime.InteropServices;

namespace Pustok0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        PustokDbContext _db { get; }

        public BlogController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _db.Blogs.Select(b => new BlogListItemVM
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Authors = b.BlogAuthors.Select(a=>a.Author).ToList(),
                Tags = b.BlogTags.Select(t=>t.Tag).ToList(),
            }).ToListAsync();
            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM vm)
        {
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            if (!ModelState.IsValid) return View(vm);
            Blog blog = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                BlogAuthors = vm.AuthorId.Select(id => new BlogAuthor
                {
                    AuthorId = id,
                }).ToList(),
                BlogTags = vm.TagId.Select(id => new BlogTag
                {
                    TagId = id,
                }).ToList(),
            };
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blogs.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            BlogUpdateVM vm = new BlogUpdateVM
            {
                Title = data.Title,
                Description = data.Description,
                AuthorId = data.BlogAuthors?.Select(x => x.AuthorId).ToList(),
                TagId = data.BlogTags?.Select(x => x.TagId).ToList(),
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, BlogUpdateVM vm)
        {
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Description = vm.Description;
            //data.BlogAuthors = vm.AuthorId.Select(id=> new BlogAuthor
            //{
            //    AuthorId = id,
            //}).ToList();
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
