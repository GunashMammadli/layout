using System.ComponentModel.DataAnnotations;

namespace Pustok0.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
