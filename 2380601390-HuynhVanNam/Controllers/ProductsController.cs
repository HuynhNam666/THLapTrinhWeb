using _2380601390_HuynhVanNam.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2380601390_HuynhVanNam.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
