using AutoMapper;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project.Entities.DTOs;
using Project.Entities.Models;
using Project.Entities.Request;
using Project.Entities.VMs;
using Project.Services.Abstracts;
using Project.Services.Contracts;
using Project.Services.Extension;
using Repository.Context;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace Project.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Editor,Blogger")]
    public class BlogController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IBlogService _blogManager;
        private readonly ICategoryService _categoryManager;
        private readonly UserService userService;
        private readonly AppDbContext _context;
        public BlogController(IServiceManager manager, UserManager<BlogUser> userManager, IBlogService blogManager, ICategoryService categoryManager, UserService userService, AppDbContext context)
        {
            _manager = manager;
            _userManager = userManager;
            _blogManager = blogManager;
            _categoryManager = categoryManager;
            this.userService = userService;
            _context = context;
        }

        public async Task<IActionResult> Index(RequestParameters parameters, int page = 1)
        {
            int pageSize = 10;
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            var blogs = _manager.IBlogService.GetAllBlogDTOsUserAndCatName(false)
                .Where(x => x.Checked == Entities.Enums.Checked.Approved);

            if (parameters.search != null)
                blogs = blogs.BySearch(parameters.search);

            var blogsWithFavoriteStatus = await userService.GetBlogsWithFavoriteStatusAsync(userId, blogs.ToList());

            var pagedBlogs = blogs.ToPagedList(page, pageSize);

            // **En Çok Okunanlar için Listeleme (isClicked'e Göre Sıralama)**
            var mostReadBlogs = blogs.OrderByDescending(x => x.isClicked).Take(10).ToList();

            // ViewBag ile View'e Gönder
            ViewBag.MostReadBlogs = mostReadBlogs;

            return View(pagedBlogs);
        }



        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategoriesSelectList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogDTO model)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                BlogDTO blogDTO = new BlogDTO
                {
                    Title = model.Title,
                    Description = model.Description,
                    UserId = userId,
                    // Assuming you have a property to hold the category IDs in BlogDTO
                    CategoryIds = model.CategoryIds // Pass selected category IDs to the BlogDTO
                };
                //model.UserId = userId;
                _manager.IBlogService.CreateOneBlog(blogDTO);
                TempData["Success"] = $"{model.Title} Adlı blog onay bekliyor";
                return RedirectToAction("MyAccount","Account");
            }

            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var blog = _manager.IBlogService.GetOneBlog(id, false);
            if (blog is null)
                return NotFound();
            var user = await _manager.UserManager.FindByIdAsync(blog.UserId);

            ViewBag.User = user;

            // isClicked değerini artır
            blog.isClicked++;

            // Değişikliği veritabanına kaydet
            var blogDTO = _manager.IBlogService.BlogToDTO(blog);

            _manager.IBlogService.UpdateOneBlog(blogDTO);

            return View(blog);
        }

        [HttpGet]
        public IActionResult CreateComment(int blogid)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Unauthorized();
            var comment = new CommentDTO { BlogId = blogid, UserId = userId };
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }
        [HttpPost]
        public IActionResult CreateComment(CommentDTO comment)
        {

            if (ModelState.IsValid)
            {
                _manager.CommentService.CreateComment(comment);
                TempData["Success"] = $"{comment.Description} Adlı yorum onay bekliyor";
                return RedirectToAction("Index");
            }

            //var Blog = _blogManager.GetOneBlog(comment.BlogId, true);
            //Blog.Comments.Add(new Comment() { BlogId = comment.BlogId, Description = comment.Description, UserId = comment.UserId });
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddComment(CommentDTO model)
        {
            if (model is not null)
            {
                var comment = new CommentDTO
                {
                    BlogId = model.BlogId,
                    UserId = model.UserId,
                    Description = model.Description,
                };
                _manager.CommentService.CreateComment(comment);
                TempData["Success"] = $"{comment.Description} Adlı yorum onay bekliyor";
                return RedirectToAction("Detail","Blog", new { id = model.BlogId});
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            var blog = _blogManager.GetOneBlog(id, true);
            if (blog == null)
            {
                return NotFound();
            }

            var users = _userManager.Users.ToList(); // Kullanıcıları alıyoruz
            var categories = _categoryManager.GetCategories(true); // Kategorileri alıyoruz

            // Kullanıcıları ve kategorileri ViewBag ile gönderiyoruz
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            // BlogDTO'yu alıyoruz
            var blogDTO = _blogManager.BlogToDTO(blog);

            // Seçili kategorileri blogDTO üzerinden alıyoruz
            ViewBag.SelectedCategories = blogDTO.CategoryIds;

            return View(blogDTO); // BlogDTO'yu view'a gönderiyoruz
        }


        [HttpPost]
        public IActionResult EditBlog(BlogDTO blogDTO)
        {
            if (!ModelState.IsValid)
            {
                var users = _userManager.Users.ToList();
                var categories = _categoryManager.GetCategories(true);

                ViewBag.Users = new SelectList(users, "Id", "UserName");
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

                // Seçili kategorileri ViewBag'e ekliyoruz
                ViewBag.SelectedCategories = blogDTO.CategoryIds;

                return View(blogDTO);
            }

            // Blogu güncelliyoruz
            _blogManager.UpdateOneBlog(blogDTO);
            return RedirectToAction("MyAccount", "Account");
        }

        private SelectList GetCategoriesSelectList()
        {
            return new SelectList(_manager.CategoryService.GetCategories(true), "Id", "CategoryName");
        }
       

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int blogId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            var user = await _userManager.FindByIdAsync(userId); // Fetch user data from the UserManager

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var blog = _blogManager.GetOneBlog(blogId, true); // Fetch the blog from the database
            if (blog == null)
            {
                return NotFound();
            }

            // Check if the blog is already in the user's favorite blogs
            var existingFavorite = user.FavoriteBlogs.FirstOrDefault(f => f.BlogId == blogId);

            if (existingFavorite != null)
            {
              
                user.FavoriteBlogs.Remove(existingFavorite);
                await _userManager.UpdateAsync(user); 

                TempData["SuccessMessage"] = "Blog removed from favorites!";
            }
            else
            {
                
                user.FavoriteBlogs.Add(new FavBlog
                {
                    UserId = user.Id,
                    BlogId = blogId
                });
                await _userManager.UpdateAsync(user); 

                TempData["SuccessMessage"] = "Blog added to favorites!";
            }

            await _userManager.UpdateAsync(user); // Update the user with the new favorite blog

            TempData["SuccessMessage"] = "Blog added to your favorites!";
            return RedirectToAction("MyFavorites","Account");

        }

        [HttpPost]
        public async Task<IActionResult> AddToAuthorFav(string authorId)
        {
            var loginUser = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = await _userManager.FindByIdAsync(loginUser); 

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var author = await _userManager.FindByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }

     
            var existingFavorite = user.FavoriteAuthors.FirstOrDefault(fa => fa.AuthorId == authorId);

            if (existingFavorite != null)
            {
                _context.FavAuthors.Remove(existingFavorite);
                await _context.SaveChangesAsync(); 
                TempData["SuccessMessage"] = "Yazar favorilerinizden çıkarıldı!";
            }
            else
            {
              
                var favAuthor = new FavAuthor
                {
                    UserId = user.Id,
                    AuthorId = authorId
                };

               
                _context.FavAuthors.Add(favAuthor);
                await _context.SaveChangesAsync(); 
                TempData["SuccessMessage"] = "Yazar favorilerinize eklendi!";
            }

            return RedirectToAction("MyFavorites","Account"); 
        }


    }
}
