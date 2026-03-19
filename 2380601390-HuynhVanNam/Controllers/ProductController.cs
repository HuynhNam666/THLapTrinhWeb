using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _2380601390_HuynhVanNam.Models;
using Microsoft.EntityFrameworkCore;

namespace _2380601390_HuynhVanNam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // CREATE
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var path = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var path = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}