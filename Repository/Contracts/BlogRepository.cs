using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Enums;
using Project.Repositories.Abstracts;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Contracts
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Blog> ApprovedBlogs(Checked ischecked, bool trackChanges)
        {
            return FindAllCondition(x => x.Checked == ischecked, trackChanges);
        }

        public void CreateOneBlog(Blog blog) => Create(blog);


        public void DeleteOneBlog(Blog blog) => Delete(blog);

        public IQueryable<Blog> GetAllBlogs(bool trackChanges)
        {
            var blogs = FindAll(trackChanges);

            return blogs.Include(b => b.BlogCategories) // Include the BlogCategories
                        .ThenInclude(bc => bc.Category); // Include the Category related to each BlogCategory
        }
        public Blog? GetOneBlog(int id, bool trackChanges) => FindById(id, trackChanges);

        public void UpdateOneBlog(Blog blog) => Update(blog);
        public int Count() => FindAll(false).Count();
    }
}
