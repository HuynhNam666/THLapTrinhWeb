using _2380601390_HuynhVanNam.Data;
using _2380601390_HuynhVanNam.Extensions;
using _2380601390_HuynhVanNam.Models;
using _2380601390_HuynhVanNam.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace _2380601390_HuynhVanNam.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string CartKey = "CART";

        public OrdersController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();

            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();

            if (!cart.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng đang trống.");
            }

            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("", $"Sản phẩm ID {item.ProductId} không tồn tại.");
                    continue;
                }

                if (product.Stock < item.Quantity)
                {
                    ModelState.AddModelError("", $"Sản phẩm {product.Name} không đủ tồn kho.");
                }
            }

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var order = new Order
            {
                CustomerName = model.CustomerName,
                Phone = model.Phone,
                Address = model.Address,
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(x => x.Price * x.Quantity),
                OrderDetails = cart.Select(x => new OrderDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.Price
                }).ToList()
            };

            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock -= item.Quantity;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartKey);

            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            return View(orders);
        }
    }
}