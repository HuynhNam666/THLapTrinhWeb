using _2380601390_HuynhVanNam.Models;
using _2380601390_HuynhVanNam.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2380601390_HuynhVanNam.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // Form thêm
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // Thêm sản phẩm
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    product.ImageUrl = await SaveImage(imageFile);
                }

                await _productRepository.AddAsync(product);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(product);
        }

        // Lưu ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            var path = Path.Combine("wwwroot/images", image.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return "/images/" + image.FileName;
        }

        // Xem chi tiết
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // Form cập nhật
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        // Cập nhật
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageFile)
        {
            if (id != product.Id)
                return NotFound();

            var existingProduct = await _productRepository.GetByIdAsync(id);

            if (existingProduct == null)
                return NotFound();

            if (imageFile != null)
            {
                existingProduct.ImageUrl = await SaveImage(imageFile);
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;

            await _productRepository.UpdateAsync(existingProduct);

            return RedirectToAction(nameof(Index));
        }

        // Xóa
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}