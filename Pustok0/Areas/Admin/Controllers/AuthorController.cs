using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.AuthorVM;
using Pustok0.Context;
using Pustok0.Models;
using Pustok0.ViewModels;

namespace Pustok0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        PustokDbContext _db { get; }

        public AuthorController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var author = await _db.Authors.Select(s => new AuthorListItemVM
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname, 
            }).ToListAsync();
            return View(author);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Author author = new Author
            {
                Name = vm.Name,
                Surname = vm.Surname,
            };
            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            _db.Authors.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            AuthorUpdateVM vm = new AuthorUpdateVM
            {
                Name = data.Name,
                Surname = data.Surname,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, AuthorUpdateVM vm)
        {
            if (id == null) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.Surname = vm.Surname;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
