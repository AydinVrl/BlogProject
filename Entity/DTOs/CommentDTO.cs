using Microsoft.AspNetCore.Identity;
using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.DTOs
{
    public record CommentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }
        public int BlogId { get; set; }
        public Checked Checked { get; set; }

        public string UserId { get; set; }
    }
}
