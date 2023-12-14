using Pustok0.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pustok0.Areas.Admin.ViewModels.ProductVM
{
    public class ProductCreateVM
    {
        [MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(128)]
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort StockCount { get; set; }
        public IFormFile CardImage { get; set; }
        public IFormFile HoverImage { get; set; }
        [Range(0, 5)]
        public float Review { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<int>? TagId { get; set; }
        public IEnumerable<int>? AuthorId { get; set; }
    }
}
