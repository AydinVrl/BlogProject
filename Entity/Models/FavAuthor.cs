using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class FavAuthor : BaseClass
    {
        
        public string UserId { get; set; } //Favorite user
        public virtual BlogUser User { get; set; }

        public string AuthorId { get; set; } //Author added to favorites
        public virtual BlogUser Author { get; set; }
    }
}
