namespace Pustok0.Models
{
    public class Blog
    {      
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
        public ICollection<BlogAuthor>? BlogAuthors { get; set; }
    }
}
