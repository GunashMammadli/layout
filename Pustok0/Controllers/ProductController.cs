using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok0.Areas.Admin.ViewModels.CommonVM;
using Pustok0.Areas.Admin.ViewModels.ProductVM;
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

        public async Task<IActionResult> Index(string? q, List<int>? catIds, List<int>? authorIds, List<int>? tagIds)
        {
			ViewBag.Categories = _context.Categories.Include(c => c.Products);
			ViewBag.Colors = _context.Authors;
			ViewBag.Colors = _context.Tags;
			var query = _context.Products.AsQueryable();
			if (!string.IsNullOrWhiteSpace(q))
			{
				query = query.Where(p => p.Title.Contains(q));
			}
			if (catIds != null && catIds.Any())
			{
				query = query.Where(p => catIds.Contains(p.CategoryId));
			}
			if (authorIds != null && authorIds.Any())
			{
				var prodIds = _context.ProductAuthors.Where(c => authorIds.Contains(c.AuthorId)).Select(c => c.ProductId).AsQueryable();
				query = query.Where(p => prodIds.Contains(p.Id));
			}
            if (tagIds != null && tagIds.Any())
			{
				var prodIds = _context.ProductTags.Where(c => tagIds.Contains(c.TagId)).Select(c => c.ProductId).AsQueryable();
				query = query.Where(p => prodIds.Contains(p.Id));
			}
			return View(query.Select(p => new AdminProductListItemVM
			{
				Id = p.Id,
				Category = p.Category,
				Discount = p.Discount,
                Title = p.Title,
                CardImage = p.CardImage,
                HoverImage = p.HoverImage,
                Price = p.Price,
			}));
		}

		//public async Task<IActionResult> ProductFilter(string? q, List<int>? catIds, List<int>? authorIds, List<int>? tagIds, int page = 1, int take = 8)
		//{
		//	var query = _context.Products.AsQueryable();
		//	if (!string.IsNullOrWhiteSpace(q))
		//	{
		//		query = query.Where(p => p.Title.Contains(q));
		//	}
		//	if (catIds != null && catIds.Any())
		//	{
		//		query = query.Where(p => catIds.Contains(p.CategoryId));
		//	}
		//	if (authorIds != null && authorIds.Any())
		//	{
		//		var prodIds = _context.ProductAuthors.Where(c => authorIds.Contains(c.AuthorId)).Select(c => c.ProductId).AsQueryable();
		//		query = query.Where(p => prodIds.Contains(p.Id));
		//	}
		//	if (tagIds != null && tagIds.Any())
		//	{
		//		var prodIds = _context.ProductTags.Where(c => tagIds.Contains(c.TagId)).Select(c => c.ProductId).AsQueryable();
		//		query = query.Where(p => prodIds.Contains(p.Id));
		//	}
		//	int count = await query.CountAsync();
		//	PaginatonVM<IEnumerable<AdminProductListItemVM>> pag = new PaginatonVM<IEnumerable<AdminProductListItemVM>>(count, page, (int)Math.Ceiling((decimal)count / take), query.AddPagination(page, take).Select(p => new AdminProductListItemVM
		//	{
		//		Id = p.Id,
		//		Category = p.Category,
		//		Discount = p.Discount,
		//		Title = p.Title,
		//		CardImage = p.CardImage,
		//		HoverImage = p.HoverImage,
		//		Price = p.Price,
		//	}));
		//	return PartialView("_ProductPaginationPartial", pag);
		//}

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
                basket.Remove(existItem);
            }
            
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
