using System.ComponentModel.DataAnnotations;

namespace Pustok0.ViewModels
{
    public class SliderListItemVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonText { get; set; }
    }
}
