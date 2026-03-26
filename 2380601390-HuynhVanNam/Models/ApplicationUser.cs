using Microsoft.AspNetCore.Identity;

namespace _2380601390_HuynhVanNam.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
