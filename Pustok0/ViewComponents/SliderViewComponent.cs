using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Context;
using Pustok0.ViewModels;

namespace Pustok0.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        PustokDbContext _db { get; }

        public SliderViewComponent(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _db.Sliders.Select(s => new SliderListItemVM
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                ButtonText = s.ButtonText,
                Image = s.Image
            }).ToListAsync());
        }
    }
}
