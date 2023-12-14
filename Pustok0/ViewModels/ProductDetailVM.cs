using Pustok0.Models;

namespace Pustok0.ViewModels
{
    public class ProductDetailVM
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
    }
}
