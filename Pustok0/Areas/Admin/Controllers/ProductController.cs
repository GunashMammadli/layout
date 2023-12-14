using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.CommonVM;
using Pustok0.Areas.Admin.ViewModels.ProductVM;
using Pustok0.Context;
using Pustok0.Helpers;
using Pustok0.Models;
using Pustok0.ViewModels;
using System.Security.Policy;

namespace Pustok0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _environment { get; }

        public ProductController(PustokDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Products.Take(4).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CardImage = p.CardImage,
                HoverImage = p.HoverImage,
                Price = p.Price,
                Discount = p.Discount,
                StockCount = p.StockCount,
                Review = p.Review,
                IsDeleted = p.IsDeleted,
                Category = p.Category,
                Tags = p.ProductTags.Select(pt => pt.Tag).ToList(),
                Authors = p.ProductAuthors.Select(pt => pt.Author).ToList(),
            }).ToListAsync();

            int totalCount = await _db.Products.CountAsync();
            PaginationVM<IEnumerable<AdminProductListItemVM>> pag = new(totalCount, 1, 4, items);

            return View(pag);

        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            if (vm.StockCount<0)
            {
                ModelState.AddModelError("StockCount", "Stock Count must be grater than 0.");
            }
            if (!ModelState.IsValid) return View(vm);
            if (!await _db.Categories.AnyAsync(c => c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                return View(vm);
            }
            Product product = new Product
            {
                Title = vm.Title,
                Description = vm.Description,
                CardImage = await vm.CardImage.SaveAsync(PathConstants.product),
                HoverImage = await vm.HoverImage.SaveAsync(PathConstants.product),
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                Discount = vm.Discount,
                StockCount = vm.StockCount,
                Review = vm.Review,
                ProductAuthors = vm.AuthorId.Select(id => new ProductAuthor
                {
                    AuthorId = id,
                }).ToList(),
                ProductTags = vm.TagId.Select(id => new ProductTag
                {
                    TagId = id,
                }).ToList()
            };
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();
            _db.Products.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();
            ProductUpdateVM vm = new ProductUpdateVM
            {
                Title = data.Title,
                Description = data.Description,
                Price = data.Price,
                Discount = data.Discount,
                StockCount = data.StockCount,
                Review = data.Review,
                CategoryId = data.CategoryId,
                AuthorId = data.ProductAuthors.Select(x => x.AuthorId).ToList(),
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Authors = new SelectList(_db.Authors, "Id", "Name");
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.Price = vm.Price;
            data.Discount = vm.Discount;
            data.StockCount = vm.StockCount;
            data.Review = vm.Review;
            data.CategoryId = vm.CategoryId;
            data.CardImage = await vm.CardImage.SaveAsync(PathConstants.product);
            data.HoverImage = await vm.HoverImage.SaveAsync(PathConstants.product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {
            var items = _db.Products.Skip((page - 1) * count).Take(count).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Description = p.Description,
                Price = p.Price,
                Discount = p.Discount,
                Review = p.Review,
                Title = p.Title,
                StockCount = p.StockCount,
                CardImage = p.CardImage,
                HoverImage = p.HoverImage,
                Category = p.Category,
                Tags = p.ProductTags.Select(pt => pt.Tag).ToList(),
                Authors = p.ProductAuthors.Select(pt => pt.Author).ToList(),
            });
            int totalCount = await _db.Products.CountAsync();
            PaginationVM<IEnumerable<AdminProductListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);

            return PartialView("_ProductPaginationPartial", pag);
        }


    }
}
