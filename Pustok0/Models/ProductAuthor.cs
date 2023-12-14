namespace Pustok0.Models
{
    public class ProductAuthor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AuthorId { get; set; }
        public Product Product { get; set; }
        public Author Author { get; set; }
    }
}
