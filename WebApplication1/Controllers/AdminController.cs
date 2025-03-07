using Castle.Core.Resource;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Entities.DTOs;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.Entities.VMs;
using Project.Services.Abstracts;
using Project.Services.Contracts;

namespace Project.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBlogService _blogManager;
        private readonly ICategoryService _categoryManager;
        private readonly ICommentService _commentManager;
        

        public AdminController(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager, IBlogService bookManager, ICategoryService categoryManager, ICommentService commentManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _blogManager = bookManager;
            _categoryManager = categoryManager;
            _commentManager = commentManager;
        }
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Index()
        {
            ViewBag.blogCount = _blogManager.GetBlogCount();
            ViewBag.userCount = _userManager.Users.Count();
            ViewBag.categoryCount = _categoryManager.GetCategoryCount();
            ViewBag.commentCount = _commentManager.GetCommentCount();

            var users = _userManager.Users.ToList();

            return View();
        }
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> GetBlogs(Checked? isChecked)
        {
            var blogs = _blogManager.GetAllBlogs(true);

            if (isChecked.HasValue)
            {
                blogs = blogs.Where(b => b.Checked == isChecked.Value).ToList();
            }

            ViewBag.CheckedStatus = isChecked; // Store selected value in ViewBag
            return View(blogs);
        }

        [Authorize(Roles = "Editor,Admin")]
        public IActionResult GetCategory()
        {
            var categories = _categoryManager.GetCategories(true);

            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult CreateCategory(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDTO);
            }
            _categoryManager.CreateCategory(categoryDTO);
            return RedirectToAction("GetCategory");
        }


        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult CreateBlog()
        {
            var users = _userManager.Users.ToList(); // Fetch users
            var categories = _categoryManager.GetCategories(true);
            ViewBag.Users = new SelectList(users, "Id", "UserName"); // Pass users to view
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult CreateBlog(BlogDTO blogDTO)
        {
            if (!ModelState.IsValid)
            {
                var users = _userManager.Users.ToList(); // Fetch users
                var categories = _categoryManager.GetCategories(true);
                ViewBag.Users = new SelectList(users, "Id", "UserName"); // Pass users to view
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                return View(blogDTO);
            }
            _blogManager.CreateOneBlog(blogDTO);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult EditBlog(int id)
        {
            var blog = _blogManager.GetOneBlog(id, true);
            if (blog == null)
            {
                return NotFound();
            }

            var users = _userManager.Users.ToList();
            var categories = _categoryManager.GetCategories(true);

            ViewBag.Users = new SelectList(users, "Id", "UserName");

            // Pass multiple selected categories
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName,
                Selected = blog.BlogCategories.Any(bc => bc.BlogId == c.Id) // Check selected categories
            }).ToList();

            var blogDTO = _blogManager.BlogToDTO(blog);
            blogDTO.CategoryIds = blog.BlogCategories.Select(c => c.BlogId).ToList(); // Populate multiple categories

            return View(blogDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult EditBlog(BlogDTO blogDTO)
        {
            if (!ModelState.IsValid)
            {
                var users = _userManager.Users.ToList();
                var categories = _categoryManager.GetCategories(true);

                ViewBag.Users = new SelectList(users, "Id", "UserName");
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName,
                    Selected = blogDTO.CategoryIds.Contains(c.Id) // Preserve selected categories
                }).ToList();

                return View(blogDTO);
            }

            _blogManager.UpdateOneBlog(blogDTO); // Ensure UpdateOneBlog can handle multiple categories
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Delete(int id)
        {
            var blog = _blogManager.GetOneBlog(id, true);
            if (blog == null)
            {
                return NotFound();
            }

            _blogManager.DeleteOneBlog(id);
            return RedirectToAction("Index");
        }

        public IActionResult GetComments()
        {
            var comments = _commentManager.GetAllCommentDTOs(true);
            return View(comments);
        }

        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult CreateComment()
        {
            var users = _userManager.Users.Select(u => new { Id=u.Id, UserName =u.UserName }).ToList();
            var blogs = _blogManager.GetAllBlogs(true).Select(b => new {Id =b.Id, Title =b.Title}).ToList();

            ViewBag.Blogs = new SelectList(blogs, "Id", "Title");
            ViewBag.Users = new SelectList(users, "Id", "UserName");


            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult CreateComment(CommentDTO comment)
        {
            if (!ModelState.IsValid)
            {
                return View(comment);
            }
            _commentManager.CreateComment(comment);
            //var Blog = _blogManager.GetOneBlog(comment.BlogId, true);
            //Blog.Comments.Add(new Comment() { BlogId = comment.BlogId, Description = comment.Description, UserId = comment.UserId });
            return RedirectToAction("GetBlogs");
        }


        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult EditComment(int id)
        {
            var comment = _commentManager.GetCommentById(id, true);
          
            if (comment == null)
                return NotFound();

            var users = _userManager.Users.Select(u => new { Id = u.Id, UserName = u.UserName }).ToList();
            var blogs = _blogManager.GetAllBlogs(true).Select(b => new { Id = b.Id, Title = b.Title }).ToList();

            ViewBag.Blogs = new SelectList(blogs, "Id", "Title", comment.BlogId);
            ViewBag.Users = new SelectList(users, "Id", "UserName", comment.UserId);

            return View(comment);
        }


        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult EditComment(CommentDTO updatedComment)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedComment);
            }

            var existingComment = _commentManager.GetCommentById(updatedComment.Id, true);
            if (existingComment == null)
                return NotFound();

            _commentManager.EditComment(updatedComment.Id, updatedComment);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _commentManager.GetCommentById(commentId, true);
            if (comment == null)
                return NotFound();

            _commentManager.DeleteComment(commentId);

            return RedirectToAction("GetBlogs");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserVM>();

            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                userRoles.Add(new UserVM
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Roles = roles.ToList()
                });
            }

            return View(userRoles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleManage(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new ManagerUserRoleVM
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(role => new RoleSelectionVM
                {
                    RoleName = role,
                    IsSelected = userRoles.Contains(role)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleManage(ManagerUserRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var userRoles = await _userManager.GetRolesAsync(user);

            //Yeni Rol Ekle
            foreach (var role in model.Roles.Where(r => r.IsSelected && !userRoles.Contains(r.RoleName)))
            {
                await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            //Yeni Rol Silme
            foreach (var role in model.Roles.Where(r => !r.IsSelected && userRoles.Contains(r.RoleName)))
            {
                await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }

            return RedirectToAction("GetUsers");



        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult DeleteCategory(int catId)
        {
            var category = _categoryManager.GetOneCategory(catId);
            if (category is null)
                return NotFound();

            _categoryManager.DeleteCategory(catId);

            return RedirectToAction("GetCategory");
        }

        [HttpGet]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult EditCategory(int id)
        {
            var category = _categoryManager.GetCategoryDTOById(id);

            if (category is null)
                return NotFound();
            

            return View(category);

        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult EditCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return View(category);

            int catId = category.Id.Value;
            var existingCat = _categoryManager.GetCategoryDTOById(catId);

            if (existingCat is null)
                return NotFound();

            _categoryManager.UpdateCategory(catId,category);

            return RedirectToAction("GetCategory");

        }

    }
}
