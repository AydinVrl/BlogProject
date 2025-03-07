using Entity.Enums;
using Entity.Models;
using Project.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Extension
{
    public static class BlogServiceExtension
    {
        public static IQueryable<Blog> StatusActive(this IQueryable<Blog> blog, Status? status)
        {
            return blog.Where(b => b.Status == status);
        }

        public static IQueryable<Blog> ByCatId(this IQueryable<Blog> blogs, int? CatId)
        {
            return blogs.Where(b => b.Id == CatId);
        }

        public static IQueryable<Blog> ByStatusAndCatId(this IQueryable<Blog> blogs, int? CatId, Status? status)
        {
            return blogs.Where(b => b.Id == CatId && b.Status == status);
        }

        public static IEnumerable<BlogDTO> BySearch(this IEnumerable<BlogDTO> blogs, string? search)
        {
            return blogs.Where(x => x.Title.ToLower().Contains(search.ToLower()));
        }
    }
}
