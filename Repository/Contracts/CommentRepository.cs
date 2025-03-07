using Entity.Enums;
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
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
       // private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context) : base(context)
        {
            
        }
        
        //public IEnumerable<Comment> GetCommentsByBlogId(int blogId)
        //{
        //    return _context.Comments
        //                   .Where(c => c.BlogId == blogId)
        //                   .ToList();
        //}

        public IQueryable<Comment> GetAllComments(bool trackChanges) => FindAll(trackChanges);

        public int Count() => FindAll(false).Count();


        public IEnumerable<Comment> GetCommentsByBlogId(int blogId, bool trackChanges, Status? status = null)
        {
            if (status is null)
                return FindAllCondition(x => x.BlogId == blogId, trackChanges);
            else
                return FindAllCondition(x => x.BlogId == blogId, trackChanges).Where(x => x.Status == status);
        }

        public Comment? GetOneComment(int blogId, bool trackChanges, Status? status = null)
        {
            if (status is null)
                return FindByCondition(x => x.BlogId == blogId, trackChanges);
            else
                return FindByCondition(x => x.BlogId == blogId, trackChanges);
        }

        public IEnumerable<Comment> GetCommentsByUserId(string userId, bool trackChanges, Status? status = null)
        {
            if (status is null)
                return FindAllCondition(x => x.UserId == userId, trackChanges);
            else
                return FindAllCondition(x => x.UserId == userId, trackChanges).Where(x => x.Status == status);
        }

        public void CreateComment(Comment comment) => Create(comment);


        public void UpdateComment(Comment comment) => Update(comment);  

        public void DeleteComment(Comment comment) => Delete(comment);

    }
}
