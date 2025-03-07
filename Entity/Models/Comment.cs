using Entity.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Comment : BaseClass
    {
        public string? Description { get; set; }

        public Checked Checked { get; set; } = Checked.Pending;
        public int BlogId { get; set; }  // Foreign key reference to Blogs
        public virtual Blog Blog { get; set; }
        public string UserId { get; set; }
        public virtual BlogUser? User { get; set; }
    }
}
