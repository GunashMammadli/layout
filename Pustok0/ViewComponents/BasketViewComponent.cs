using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok0.Context;
using Pustok0.ViewModels.BasketVM;

namespace Pustok0.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        PustokDbContext _context { get; }

        public BasketViewComponent(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var products = _context.Products.Where(p => items.Select(i => i.Id).Contains(p.Id));
            List<BasketProductItemVM> basketItems = new();
            foreach (var item in products)
            {
                basketItems.Add(new BasketProductItemVM
                {
                    Id = item.Id,
                    Discount = item.Discount,
                    Image = item.CardImage,
                    Name = item.Title,
                    Price = item.Price,
                    Count = items.FirstOrDefault(x => x.Id == item.Id).Count
                });
            }
            return View(basketItems);
        }
    }
}
