namespace Pustok0.ViewModels.BasketVM
{
    public class BasketProductItemVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public float Discount { get; set; }
    }
}
