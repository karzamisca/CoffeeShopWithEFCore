using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        public List<Product> Products { get; set; }

        // Constructor to initialize Products
        public Order()
        {
            Products = new List<Product>();
        }
    }
}
