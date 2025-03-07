using AutoMapper;
using Entity.Models;
using Microsoft.Identity.Client;
using Project.Entities.DTOs;
using Project.Repositories.UnitOfWork;
using Project.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public CategoryService(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void CreateCategory(CategoryDTO categoryDTO)
        {
            var createCat = new Category() { CategoryName=categoryDTO.CategoryName};
            //var createCat = _mapper.Map<Category>(categoryDTO);
            _manager.CategoryRepository.Create(createCat);
            _manager.Save();
        }

        public void DeleteCategory(int id)
        {
            var deleteCat = _manager.CategoryRepository.FindById(id, true);
            _manager.CategoryRepository.Delete(deleteCat);
            _manager.Save();
        }

        public IEnumerable<Category> GetCategories(bool trackChanges) => _manager.CategoryRepository.FindAll(trackChanges).ToList();

        public CategoryDTO GetCategoryDTOById(int id)
        {
            var cat = _manager.CategoryRepository.FindById(id, true);

            return new CategoryDTO {  Id = cat.Id ,CategoryName = cat.CategoryName};

            //return new CategoryDTO { Id = cat.Id, CategoryName = cat.CategoryName, Status = cat.Status };
        }

        public void UpdateCategory(int id, CategoryDTO category)
        {

            var catUpdate = _manager.CategoryRepository.GetAllCategories(true).FirstOrDefault(x => x.Id == id);

            catUpdate.CategoryName = category.CategoryName;
            _manager.CategoryRepository.Update(catUpdate);
            _manager.Save();


        }
        public int GetCategoryCount()
        {
            return _manager.CategoryRepository.Count();
        }
        public Category GetOneCategory(int id)
        {
            return _manager.CategoryRepository.FindById(id, true);
        }

       
       
    }
}
