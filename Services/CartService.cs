using System.Collections.Generic;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{

    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext?.Session ?? new FakeSession();

        public List<Product> GetCart()
        {
            var cart = Session.GetObjectFromJson<List<Product>>("Cart");
            return cart ?? new List<Product>();
        }

        public void SaveCart(List<Product> cart)
        {
            Session.SetObjectAsJson("Cart", cart);
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

    // A fake session implementation to use when HttpContext.Session is null
    public class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;

        public string Id => string.Empty;

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_sessionStorage.TryGetValue(key, out var tempValue))
            {
                value = tempValue;
                return true;
            }
            value = Array.Empty<byte>();
            return false;
        }
    }
}
