using System.ComponentModel.DataAnnotations;

namespace Pustok0.Models
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
        public ICollection<ProductAuthor>? ProductAuthors { get; set; }
        public ICollection<BlogAuthor>? BlogAuthors { get; set; }
    }
}
