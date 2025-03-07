using Entity.Abstract;
using Entity.Enums;
using Microsoft.AspNetCore.Identity;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Blog : BaseClass
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Checked Checked { get; set; } = Checked.Pending;
        public virtual ICollection<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();

        public string UserId { get; set; }
        public virtual BlogUser? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public bool isDraft { get; set; }
        public virtual ICollection<FavBlog> FavoriteBlogs { get; set; } = new List<FavBlog>();

        public int isClicked { get; set; }
    }
}
