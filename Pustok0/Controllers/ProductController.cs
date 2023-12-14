using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Context;
using Pustok0.ViewModels;

namespace Pustok0.Controllers
{
    public class ProductController : Controller
    {
        PustokDbContext _context { get; }

        public ProductController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _context.Products.Select(p => new ProductDetailVM
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
                Category = p.Category,
                Tags = p.ProductTags.Select(p=>p.Tag),
                Authors = p.ProductAuthors.Select(p=>p.Author),
            }).SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();
            return View(data);

        }
    }
}
