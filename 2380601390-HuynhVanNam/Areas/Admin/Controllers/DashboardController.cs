using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _2380601390_HuynhVanNam.Models;

namespace _2380601390_HuynhVanNam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ProductCount = _context.Products.Count();
            ViewBag.OrderCount = _context.Orders.Count();

            return View();
        }
    }
}