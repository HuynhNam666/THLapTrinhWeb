using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2380601390_HuynhVanNam.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}