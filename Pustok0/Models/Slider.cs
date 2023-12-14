using System.ComponentModel.DataAnnotations;

namespace Pustok0.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Image {  get; set; }
        [MinLength(2), MaxLength(32)]
        public string Title { get; set; }
        [MinLength(2), MaxLength(128)]
        public string Description { get; set; }
        [MinLength(2), MaxLength(16)]
        public string ButtonText { get; set; }
    }
}
