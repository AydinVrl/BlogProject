using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class BlogUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PictureUrl { get; set; }
        public virtual ICollection<FavBlog> FavoriteBlogs { get; set; } = new List<FavBlog>();
        public virtual ICollection<FavAuthor> FavoriteAuthors { get; set; } = new List<FavAuthor>();
    }
}
