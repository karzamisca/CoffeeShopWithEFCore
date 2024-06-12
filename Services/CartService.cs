using System.Collections.Generic;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Http;

namespace CoffeeShop.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Product> GetCart()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
            return cart;
        }

        public void SaveCart(List<Product> cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        public void ClearCart()
        {
            var cart = new List<Product>();
            SaveCart(cart);
        }

        public void AddToCart(Product product)
        {
            var cart = GetCart();
            var existingProduct = cart.Find(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Quantity++;
            }
            else
            {
                product.Quantity = 1;
                cart.Add(product);
            }
            SaveCart(cart);
        }

        public void IncrementItem(int productId)
        {
            var cart = GetCart();
            var item = cart.Find(p => p.Id == productId);
            if (item != null)
            {
                item.Quantity++;
                SaveCart(cart);
            }
        }

        public void DecrementItem(int productId)
        {
            var cart = GetCart();
            var item = cart.Find(p => p.Id == productId);
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                SaveCart(cart);
            }
        }

        public void RemoveItem(int productId)
        {
            var cart = GetCart();
            var item = cart.Find(p => p.Id == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }
    }
}
