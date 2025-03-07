using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Entity.Abstract;
using Project.Entities.Models;

namespace Entity.Models
{
    public class Category : BaseClass
    {
        public string CategoryName { get; set; }
        public virtual ICollection<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();
    }
}
