using _2380601390_HuynhVanNam.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2380601390_HuynhVanNam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProducts = await _context.Products.CountAsync();
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            ViewBag.TotalRevenue = await _context.Orders.SumAsync(x => (decimal?)x.TotalAmount) ?? 0;

            return View();
        }
    }
}
