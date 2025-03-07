using Entity.Models;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.VMs
{
    public class MyFavoritesVM
    {
        public List<Blog> FavoriteBlogs { get; set; } = new List<Blog>();
        public List<BlogUser> FavoriteAuthors { get; set; } = new List<BlogUser>();
    }
}
