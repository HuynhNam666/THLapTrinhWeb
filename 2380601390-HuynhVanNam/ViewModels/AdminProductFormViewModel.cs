using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _2380601390_HuynhVanNam.ViewModels
{
    public class AdminProductFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn kho")]
        [Display(Name = "Tồn kho")]
        public int Stock { get; set; }

        [Display(Name = "Ảnh hiện tại")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Chọn ảnh")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}