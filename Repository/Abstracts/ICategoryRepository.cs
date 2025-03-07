using Entity.Models;
using Project.Repositories.Abstracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Abstracts
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        int Count();
        public IEnumerable<Category> GetByIds(IEnumerable<int> categoryIds);

        IEnumerable<Category> GetAllCategories(bool trackChanges);
    }
    
}
