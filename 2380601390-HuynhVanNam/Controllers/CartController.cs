using Microsoft.AspNetCore.Mvc;
using _2380601390_HuynhVanNam.Models;
using Newtonsoft.Json;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 🛒 Lấy giỏ hàng
    List<CartItem> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        if (session != null)
            return JsonConvert.DeserializeObject<List<CartItem>>(session);

        return new List<CartItem>();
    }

    // 💾 Lưu giỏ hàng
    void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }

    // ➕ Thêm vào giỏ
    public IActionResult Add(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        var cart = GetCart();

        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item == null)
        {
            cart.Add(new CartItem
            {
                ProductId = id,
                Name = product.Name,
                Price = product.Price, // ✅ phải là decimal
                Quantity = 1,
                ImageUrl = product.ImageUrl
            });
        }
        else
        {
            item.Quantity++;
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // 📋 Xem giỏ hàng
    public IActionResult Index()
    {
        return View(GetCart());
    }

    // 💰 Thanh toán
    public IActionResult Checkout()
    {
        var cart = GetCart();

        if (!cart.Any())
            return RedirectToAction("Index");

        var userId = User.Identity?.Name;

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Details = cart.Select(c => new OrderDetail
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Price // ✅ decimal luôn
            }).ToList()
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        HttpContext.Session.Remove("Cart");

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}