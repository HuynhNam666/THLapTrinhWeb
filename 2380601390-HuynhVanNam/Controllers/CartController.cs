using _2380601390_HuynhVanNam.Data;
using _2380601390_HuynhVanNam.Extensions;
using _2380601390_HuynhVanNam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace _2380601390_HuynhVanNam.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private const string CartKey = "CART";

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            if (product.Stock <= 0)
            {
                TempData["Error"] = "Sản phẩm đã hết hàng.";
                return RedirectToAction("Index", "Products");
            }

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (existingItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }

            HttpContext.Session.SetObject(CartKey, cart);
            TempData["Success"] = "Đã thêm vào giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                HttpContext.Session.SetObject(CartKey, cart);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObject(CartKey, cart);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
