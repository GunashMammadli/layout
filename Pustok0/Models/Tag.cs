using System.ComponentModel.DataAnnotations;

namespace Pustok0.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(16)]
        public string Name { get; set; }
        public ICollection<ProductTag>? ProductTags { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
