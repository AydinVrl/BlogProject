using Project.Repositories.Abstracts;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.UnitOfWork
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RepositoryManager(AppDbContext context, ICommentRepository commentRepository, IBlogRepository blogRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            _commentRepository = commentRepository;
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
        }

        public IBlogRepository BlogRepository => _blogRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public ICommentRepository CommentRepository => _commentRepository;

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
