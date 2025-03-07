using Entity.Models;
using Project.Repositories.Abstracts;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Contracts
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public int Count() => FindAll(false).Count();
        public IEnumerable<Category> GetByIds(IEnumerable<int> categoryIds)
        {
            return _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();
        }
        public IEnumerable<Category> GetAllCategories(bool trackChanges)
        {
            return FindAll(trackChanges);
        }
    }
}
