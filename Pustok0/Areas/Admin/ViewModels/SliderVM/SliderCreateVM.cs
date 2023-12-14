using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.SliderVM
{
    public class SliderCreateVM
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonText { get; set; }
    }
}
