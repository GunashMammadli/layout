using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.TagVM
{
    public class TagUpdateVM
    {
        [MaxLength(16)]
        public string Name { get; set; }
    }
}
