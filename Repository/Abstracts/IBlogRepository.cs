using Entity.Models;
using Project.Entities.Enums;
using Project.Repositories.Abstracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Abstracts
{
    public interface IBlogRepository : IRepositoryBase<Blog>
    {
        IQueryable<Blog> GetAllBlogs(bool trackChanges);
        Blog? GetOneBlog(int id, bool trackChanges);

        IQueryable<Blog> ApprovedBlogs(Checked ischecked, bool trackChanges);

        void CreateOneBlog(Blog blog);
        void UpdateOneBlog(Blog blog);
        void DeleteOneBlog(Blog blog);
        int Count();
    }
}
