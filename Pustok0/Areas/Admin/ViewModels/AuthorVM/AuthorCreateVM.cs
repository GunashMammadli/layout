using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.AuthorVM
{
    public class AuthorCreateVM
    {
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
    }
}
