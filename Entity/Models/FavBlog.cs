using Entity.Abstract;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class FavBlog : BaseClass
    {
        public string UserId { get; set; } // Foreign key to BlogUser
        public virtual BlogUser User { get; set; }

        public int BlogId { get; set; } // Foreign key to Blog
        public virtual Blog Blog { get; set; }

        
    }
}
