using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
