using Microsoft.AspNetCore.Identity;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Abstracts
{
    public interface IServiceManager
    {
        IBlogService IBlogService { get; }
        ICategoryService CategoryService { get; }
        ICommentService CommentService { get; }

        UserManager<BlogUser> UserManager { get; }
    }
}
