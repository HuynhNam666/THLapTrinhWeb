using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2380601390_HuynhVanNam.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999999)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}