using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class BlogCategory
    {
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
