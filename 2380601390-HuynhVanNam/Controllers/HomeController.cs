using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using _2380601390_HuynhVanNam.Models;

namespace _2380601390_HuynhVanNam.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Products");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}