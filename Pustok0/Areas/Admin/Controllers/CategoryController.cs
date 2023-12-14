using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.CategoryVM;
using Pustok0.Areas.Admin.ViewModels.CommonVM;
using Pustok0.Areas.Admin.ViewModels.ProductVM;
using Pustok0.Context;
using Pustok0.Models;
using Pustok0.ViewModels;

namespace Pustok0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        PustokDbContext _db { get; }

        public CategoryController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Categories.Take(4).Select(c => new CategoryListItemVM
            {
                Id = c.Id,
                Name = c.Name,
            }).ToListAsync();

            int totalCount = await _db.Categories.CountAsync();
            PaginationVM<IEnumerable<CategoryListItemVM>> pag = new(totalCount, 1, 4, items);

            return View(pag);
        }

        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if(!ModelState.IsValid) return View(vm);
            Category category = new Category
            {
                Name = vm.Name,
            };
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            _db.Categories.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            CategoryUpdateVM vm = new CategoryUpdateVM
            {
                Name = data.Name,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CategoryPagination(int page = 1, int count = 8)
        {
            var items = _db.Categories.Skip((page - 1) * count).Take(count).Select(p => new CategoryListItemVM
            {
                Id = p.Id,
                Name = p.Name,
            });
            int totalCount = await _db.Categories.CountAsync();
            PaginationVM<IEnumerable<CategoryListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);

            return PartialView("_CategoryPaginationPartial", pag);
        }
    }
}
