using AutoMapper;
using Entity.Enums;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Project.Entities.DTOs;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.Repositories.UnitOfWork;
using Project.Services.Abstracts;
using Project.Services.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public class BlogService : IBlogService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public BlogService(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void CreateOneBlog(BlogDTO blog)
        {
            var oneBlog = new Blog() { Title = blog.Title, Description = blog.Description, UserId = blog.UserId };
            //var oneBlog = _mapper.Map<Blog>(blog);
            var categories = _manager.CategoryRepository.GetByIds(blog.CategoryIds);

            // Add the categories to the BlogCategories collection
            foreach (var category in categories)
            {
                oneBlog.BlogCategories.Add(new BlogCategory
                {
                    Blog = oneBlog,
                    Category = category
                });
            }
            _manager.BlogRepository.Create(oneBlog);
            _manager.Save();
        }

        public void DeleteOneBlog(int id)
        {
            var oneBlog = GetOneBlog(id, true);
            if (oneBlog != null)
            {
                _manager.BlogRepository.Delete(oneBlog);
                _manager.Save();
            }
        }

        public IEnumerable<BlogDTO> GetAllBlogDTOs(bool trackChanges, Status? status = null)
        {
            _mapper.Map<IEnumerable<BlogDTO>>(_manager.BlogRepository.GetAllBlogs(true));

            if (status is null)
                return _mapper.Map<IEnumerable<BlogDTO>>(_manager.BlogRepository.GetAllBlogs(true));
            else
                return _mapper.Map<IEnumerable<BlogDTO>>(_manager.BlogRepository.GetAllBlogs(true).StatusActive(status));

        }

        //Deneme
        public IEnumerable<BlogDTO> GetAllBlogDTOsUserAndCatName(bool trackChanges)
        {
            var blogs = _manager.BlogRepository
            .GetAllBlogs(trackChanges)
            .Include(b => b.BlogCategories)
            .ThenInclude(bc => bc.Category)// Kategori verisini çekiyoruz
            .Include(b => b.User);  // Kullanıcı verisini çekiyoruz

            var blogDTOs = _mapper.Map<IEnumerable<BlogDTO>>(blogs);

            foreach (var blogDTO in blogDTOs)
            {
                // Get the category names for the current blog
                var categoryNames = blogs
                    .FirstOrDefault(b => b.Id == blogDTO.Id)?
                    .BlogCategories
                    .Select(bc => bc.Category.CategoryName)
                    .ToList();

                // Assign category names to the BlogDTO
                blogDTO.CategoryNames = categoryNames;

                // Get the category Ids for the current blog
                var categoryIds = blogs
                    .FirstOrDefault(b => b.Id == blogDTO.Id)?
                    .BlogCategories
                    .Select(bc => bc.CategoryId)
                    .ToList();

                // Assign category Ids to the BlogDTO
                blogDTO.CategoryIds = categoryIds;
            }


            return blogDTOs;
        }




        public IEnumerable<BlogDTO> GetAllBlogDTOsByCatId(bool trackChanges, int? catId, Status? status = null)
        {
            if (status is null)
                return _mapper.Map<IEnumerable<BlogDTO>>(_manager.BlogRepository.GetAllBlogs(true).ByCatId(catId));
            else
                return _mapper.Map<IEnumerable<BlogDTO>>(_manager.BlogRepository.GetAllBlogs(true).ByStatusAndCatId(catId, status));
        }

        public IEnumerable<Blog> GetAllBlogs(bool trackChanges, Status? status = null)
        {
            if (status is null)
                return _manager.BlogRepository.GetAllBlogs(true).ToList();
            else
                return _manager.BlogRepository.GetAllBlogs(true).StatusActive(status).ToList();

        }
        public IEnumerable<Blog> GetCheckedBlogs(bool trackChanges, Status? status = null)
        {
            if (status is null)
                return _manager.BlogRepository.GetAllBlogs(true).Where(b=>b.Checked==Checked.Approved).ToList();
            else
                return _manager.BlogRepository.GetAllBlogs(true).Where(b => b.Checked == Checked.Approved).StatusActive(status).ToList();

        }

        public Blog? GetOneBlog(int id, bool trackChanges)
        {
            return _manager.BlogRepository.GetOneBlog(id, trackChanges);
        }

        public void UpdateOneBlog(BlogDTO blog)
        {
            var oneBlog = _mapper.Map<Blog>(blog);
            _manager.BlogRepository.Update(oneBlog);
            _manager.Save();
        }
        public int GetBlogCount()
        {
            return _manager.BlogRepository.Count();
        }

        public BlogDTO BlogToDTO(Blog blog)
        {
            return _mapper.Map<BlogDTO>(blog);
        }
    }
}
