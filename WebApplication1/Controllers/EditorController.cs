using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Entities.Models;
using Project.Services.Abstracts;

namespace Project.WebApp.Controllers
{
    public class EditorController : Controller
    {
        private readonly IBlogService _blogManager;
        private readonly ICategoryService _categoryManager;
        private readonly ICommentService _commentManager;


        public EditorController(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager, IBlogService bookManager, ICategoryService categoryManager, ICommentService commentManager)
        {

            _blogManager = bookManager;
            _categoryManager = categoryManager;
            _commentManager = commentManager;
        }
        public IActionResult Index()
        {
            ViewBag.blogCount = _blogManager.GetBlogCount();
            ViewBag.categoryCount = _categoryManager.GetCategoryCount();
            ViewBag.commentCount = _commentManager.GetCommentCount();



            return View();
        }
    }
}
