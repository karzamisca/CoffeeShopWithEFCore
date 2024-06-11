using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Services;
using CoffeeShop.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoffeeShop.Controllers
{
    public class StoreController : Controller
    {
        private readonly ProductService _productService;

        public StoreController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // POST: Store/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var cart = GetOrCreateCart();
            var product = _productService.GetProductByIdAsync(productId).Result;
            if (product != null)
            {
                cart.Add(product);
                SaveCart(cart);
            }
            return PartialView("_CartPartial", cart);
        }

        // GET: Store/GetCart
        public IActionResult GetCart()
        {
            var cart = GetOrCreateCart();
            return PartialView("_CartPartial", cart);
        }

        private List<Product> GetOrCreateCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart");
            if (cart == null)
            {
                cart = new List<Product>();
            }
            return cart;
        }

        private void SaveCart(List<Product> cart)
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
        [HttpPost]
        public IActionResult ClearCart()
        {
            // Get the cart from the session
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();

            // Clear the contents of the cart
            cart.Clear();

            // Save the empty cart back to the session
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Return the updated cart view
            return PartialView("_CartPartial", cart);
        }

        [HttpPost]
        public IActionResult IncrementItem(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
            var item = cart.FirstOrDefault(p => p.Id == productId);
            if (item != null)
            {
                item.Quantity++;
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return PartialView("_CartPartial", cart);
        }

        [HttpPost]
        public IActionResult DecrementItem(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
            var item = cart.FirstOrDefault(p => p.Id == productId);
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return PartialView("_CartPartial", cart);
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
            var item = cart.FirstOrDefault(p => p.Id == productId);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return PartialView("_CartPartial", cart);
        }

    }
}
