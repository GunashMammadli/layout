using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok0.Areas.Admin.ViewModels.SliderVM;
using Pustok0.Context;
using Pustok0.Helpers;
using Pustok0.Models;
using Pustok0.ViewModels;

namespace Pustok0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public SliderController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var slider = await _db.Sliders.Select(s => new SliderListItemVM
            {
                Id = s.Id,
                Image = s.Image, 
                Title = s.Title,
                Description = s.Description,
                ButtonText = s.ButtonText,
            }).ToListAsync();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if(!ModelState.IsValid) return View(vm);
            Slider slider = new Slider
            {
                Image = await vm.Image.SaveAsync(PathConstants.sliderImg),
                Title = vm.Title,
                Description = vm.Description,
                ButtonText = vm.ButtonText,
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            SliderUpdateVM vm = new SliderUpdateVM
            {
                Title = data.Title,
                Description = data.Description,
                ButtonText = data.ButtonText,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (id == null) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Image = await vm.Image.SaveAsync(PathConstants.sliderImg);
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.ButtonText = vm.ButtonText;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
