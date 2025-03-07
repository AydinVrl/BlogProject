using Microsoft.AspNetCore.Identity;
using Project.Entities.Models;
using Project.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public class ServiceManager : IServiceManager
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly UserManager<BlogUser> _userManager;

        public ServiceManager(IBlogService blogService, ICategoryService categoryService, ICommentService commentService, UserManager<BlogUser> userManager)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public IBlogService IBlogService => _blogService;

        public ICategoryService CategoryService => _categoryService;

        public ICommentService CommentService => _commentService;

        public UserManager<BlogUser> UserManager => _userManager;
    }
}
