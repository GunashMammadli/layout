using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.TagVM
{
    public class TagCreateVM
    {
        [MaxLength(16)]
        public string Name { get; set; }
    }
}
