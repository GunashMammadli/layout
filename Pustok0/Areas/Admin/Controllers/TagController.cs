using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.TagVM;
using Pustok0.Context;
using Pustok0.Models;
using Pustok0.ViewModels;

namespace Pustok0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        PustokDbContext _db { get; }

        public TagController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var tags = await _db.Tags.Select(s => new TagListItemVM
            {
                Id = s.Id,
                Name = s.Name,
            }).ToListAsync();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            if(!ModelState.IsValid) return View(vm);
            Tag tag = new Tag
            {
                Name = vm.Name 
            };
            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            _db.Tags.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update (int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            TagUpdateVM vm = new TagUpdateVM
            {
                Name = data.Name,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update (int? id, TagUpdateVM vm)
        {
            if (id == null) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
