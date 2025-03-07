using Microsoft.AspNetCore.Mvc;
using Project.Services.Abstracts;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceManager _manager;

        public HomeController(ILogger<HomeController> logger, IServiceManager manager)
        {
            _logger = logger;
            _manager = manager;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View(_manager.IBlogService.GetCheckedBlogs(false));
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Deneme()
        {
            return View(_manager.IBlogService.GetAllBlogDTOs(false));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
