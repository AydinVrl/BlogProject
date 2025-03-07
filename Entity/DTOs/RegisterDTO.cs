using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.DTOs
{
    public record RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Inconsistent passwords")]
        [DataType(DataType.Password)]
        public string? ConfrimPass { get; set; }
        [DisplayName("Beni Hatırla")]
        public bool IsPersistent { get; set; }
    }
}
