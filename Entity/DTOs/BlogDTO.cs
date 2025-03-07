using Entity.Models;
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
    public record BlogDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string? Description { get; set; }
        public Checked Checked { get; set; }


        public List<int> CategoryIds { get; set; }
        public List<string>? CategoryNames { get; set; }
        
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public bool isFavorite { get; set; }

        public int isClicked { get; set; }

    }
}
