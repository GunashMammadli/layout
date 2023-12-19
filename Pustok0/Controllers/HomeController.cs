using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.ProductVM;
using Pustok0.Context;
using Pustok0.Models;
using Pustok0.ViewModels;
using System.Diagnostics;

namespace Pustok0.Controllers
{
    public class HomeController : Controller
    {
        PustokDbContext _context { get; }

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM
            {
                Products = await _context.Products.Where(p => !p.IsDeleted).Select(p => new AdminProductListItemVM
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
                    Tags = p.ProductTags.Select(pt => pt.Tag).ToList(),
                    Authors = p.ProductAuthors.Select(pt => pt.Author).ToList(),
                }).ToListAsync()
            };
            return View(vm);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
