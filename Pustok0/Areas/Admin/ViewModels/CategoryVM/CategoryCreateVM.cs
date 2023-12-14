using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.CategoryVM
{
    public class CategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
