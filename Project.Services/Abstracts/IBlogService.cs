using Entity.Enums;
using Entity.Models;
using Project.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Abstracts
{
    public interface IBlogService
    {
        IEnumerable<Blog> GetAllBlogs(bool trackChanges, Status? status = null);

        IEnumerable<BlogDTO> GetAllBlogDTOs(bool trackChanges, Status? status = null);

        IEnumerable<BlogDTO> GetAllBlogDTOsByCatId(bool trackChanges, int? catId, Status? status = null);

        public IEnumerable<BlogDTO> GetAllBlogDTOsUserAndCatName(bool trackChanges);
        Blog? GetOneBlog(int id, bool trackChanges);

        void CreateOneBlog(BlogDTO blog);
        void UpdateOneBlog(BlogDTO blog);
        void DeleteOneBlog(int id);
        int GetBlogCount();
        BlogDTO BlogToDTO(Blog blog);
        public IEnumerable<Blog> GetCheckedBlogs(bool trackChanges, Status? status = null);
    }
}
