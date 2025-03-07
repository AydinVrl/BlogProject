using Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.VMs
{
    public class BlogCreateVM
    {
        [DisplayName("Başlık: ")]
        [Required(ErrorMessage = "Title is required!")]
        public string? Title { get; set; }

        [DisplayName("Açıklama: ")]
        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }

        [DisplayName("Kategori: ")]
        public IList<Category>? Categories { get; set; }
        public IList<int>? CategoryIds { get; set; }
    }
}
