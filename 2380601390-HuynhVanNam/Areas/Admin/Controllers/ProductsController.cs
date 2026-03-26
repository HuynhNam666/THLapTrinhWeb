using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2380601390_HuynhVanNam.Data;
using _2380601390_HuynhVanNam.Models;
using _2380601390_HuynhVanNam.ViewModels;

namespace _2380601390_HuynhVanNam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private async Task<List<SelectListItem>> GetCategorySelectListAsync()
        {
            return await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }

        private async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var extension = Path.GetExtension(imageFile.FileName);
            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/images/" + fileName;
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

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AdminProductFormViewModel
            {
                Categories = await GetCategorySelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectListAsync();
                return View(model);
            }

            var imageUrl = await SaveImageAsync(model.ImageFile);

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                ImageUrl = imageUrl,
                CategoryId = model.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var model = new AdminProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Categories = await GetCategorySelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminProductFormViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectListAsync();
                return View(model);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.CategoryId = model.CategoryId;

            if (model.ImageFile != null)
            {
                var imageUrl = await SaveImageAsync(model.ImageFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.ImageUrl = imageUrl;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}