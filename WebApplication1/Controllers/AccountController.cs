using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Project.Entities.DTOs;
using Project.Entities.Models;
using Project.Entities.VMs;
using Project.Services.Abstracts;
using Project.Services.Contracts;

namespace Project.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<BlogUser> _signInManager;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IServiceManager _manager;
        private readonly UserService _userService;
        public AccountController(UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager, IServiceManager manager, UserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _manager = manager;
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new BlogUser() { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var result2 = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Blog");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Giriş yapılamadı!");
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,model.IsPersistent,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Blog");
                }
                else
                {
                    ModelState.AddModelError("", "Giriş yapılamadı!");
                }
            }
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Blog");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var blogs = _manager.IBlogService.GetAllBlogDTOsUserAndCatName(true);
            var userBlogs = blogs.Where(x => x.UserId.Equals(user.Id));
            ViewBag.Blogs = userBlogs.ToList();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string userId, string firstName, string lastName, IFormFile? profileImage, string currentPassword, string newPassword, string confirmNewPassword)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError("", "The new password and confirmation do not match.");
                return View("EditUser", await _userService.GetUserByIdAsync(userId));
            }

            var result = await _userService.UpdateUserProfileAsync(userId, firstName, lastName, profileImage);

            if (result)
            {
                // Şifreyi değiştirme işlemi
                if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword))
                {
                    var passwordResult = await _userService.ChangePasswordAsync(userId, currentPassword, newPassword);
                    if (!passwordResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Password update failed.");
                        return View("EditUser", await _userService.GetUserByIdAsync(userId));
                    }
                }


                // Başarı mesajı
                ViewData["SuccessMessage"] = "Profile updated successfully.";
                return View("EditUser", await _userService.GetUserByIdAsync(userId));
            }
            else
            {
                ModelState.AddModelError("", "Profile update failed.");
                return View("EditUser", await _userService.GetUserByIdAsync(userId));
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound();
            var blogs = _manager.IBlogService.GetAllBlogDTOsUserAndCatName(false);
            var userBlog = blogs.Where(x => x.UserId == userId);

            ViewBag.Blogs = userBlog.ToList(); 
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> MyFavorites()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.Users
                .Include(u => u.FavoriteBlogs)
                .ThenInclude(fb => fb.Blog)
                .Include(u => u.FavoriteAuthors)
                .ThenInclude(fb => fb.Author)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user is null)
                return RedirectToAction("Login", "Account");

            var model = new MyFavoritesVM
            {
                FavoriteBlogs = user.FavoriteBlogs.Select(fb => fb.Blog).ToList(),
                FavoriteAuthors = user.FavoriteAuthors.Select(fa => fa.Author).ToList()
            };

            return View(model);
        }

    }
}
