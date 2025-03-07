using Entity.Enums;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Extension
{
    public static class CommentServiceExtentions
    {
        public static IQueryable<Comment> StatusActive(this IQueryable<Comment> comment, Status? status)
        {
            return comment.Where(b => b.Status == status);
        }
    }
}
