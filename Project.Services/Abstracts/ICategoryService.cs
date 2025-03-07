using Entity.Models;
using Project.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Abstracts
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories(bool trackChanges);
        CategoryDTO GetCategoryDTOById(int id);

        void CreateCategory(CategoryDTO categoryDTO);
        void UpdateCategory(int id, CategoryDTO category);
        void DeleteCategory(int id);
        int GetCategoryCount();
        Category GetOneCategory(int id);
        
    }
}
