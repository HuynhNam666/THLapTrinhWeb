using System.ComponentModel.DataAnnotations;

namespace _2380601390_HuynhVanNam.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
