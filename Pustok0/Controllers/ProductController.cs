using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok0.Context;
using Pustok0.ViewModels;
using Pustok0.ViewModels.BasketVM;

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

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();
            var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = basket.Find(b => b.Id == id);
            if (existItem == null)
            {
                basket.Add(new BasketProductAndCountVM
                {
                    Id = (int)id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });
            return Ok();
        }

        public async Task<IActionResult> RemoveBasket(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = basket.Find(b => b.Id == id);

            if (existItem != null && existItem.Count > 0)
            {
                existItem.Count--;
            }

            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });

            return Ok();
        }
    }
}
