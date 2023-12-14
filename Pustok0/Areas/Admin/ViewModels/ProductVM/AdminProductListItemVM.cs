using Pustok0.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Pustok0.Areas.Admin.ViewModels.ProductVM
{
    public class AdminProductListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public float Discount { get; set; }
        public ushort StockCount { get; set; }
        public string CardImage { get; set; }
        public string HoverImage { get; set; }
        public float Review { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
