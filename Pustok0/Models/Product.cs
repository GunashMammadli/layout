using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pustok0.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(128)]
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort StockCount { get; set; }
        public string CardImage { get; set; }
        public string HoverImage { get; set; }
        [Range(0, 5)]
        public float Review { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<ProductTag>? ProductTags { get; set; }
        public ICollection<ProductAuthor>? ProductAuthors { get; set; }

    }
}
