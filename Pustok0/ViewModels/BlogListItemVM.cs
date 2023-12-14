using Pustok0.Models;

namespace Pustok0.ViewModels
{
    public class BlogListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<Author>? Authors { get; set; }
    }
}
