using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.CategoryVM
{
    public class CategoryUpdateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
