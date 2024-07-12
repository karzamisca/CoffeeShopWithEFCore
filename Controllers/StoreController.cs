using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Services;
using CoffeeShop.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;

        public StoreController(ProductService productService, CartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // POST: Store/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product != null)
            {
                _cartService.AddToCart(product);
                var cart = _cartService.GetCart();
                return PartialView("_CartPartial", cart);
            }
            return NotFound();
        }

        // GET: Store/GetCart
        public IActionResult GetCart()
        {
            var cart = _cartService.GetCart();
            return PartialView("_CartPartial", cart);
        }

        // POST: Store/ClearCart
        [HttpPost]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            var cart = _cartService.GetCart();
            return PartialView("_CartPartial", cart);
        }

        [HttpPost]
        public IActionResult IncrementItem(int productId)
        {
            _cartService.IncrementItem(productId);
            var cart = _cartService.GetCart();
            return PartialView("_CartPartial", cart);
        }

        [HttpPost]
        public IActionResult DecrementItem(int productId)
        {
            _cartService.DecrementItem(productId);
            var cart = _cartService.GetCart();
            return PartialView("_CartPartial", cart);
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            _cartService.RemoveItem(productId);
            var cart = _cartService.GetCart();
            return PartialView("_CartPartial", cart);
        }
    }
}
