
using System.ComponentModel.DataAnnotations;

namespace _2380601390_HuynhVanNam.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
        [Display(Name = "Tên người nhận")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;
    }
}
