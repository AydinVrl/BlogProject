using Entity.Enums;
using Entity.Models;
using Project.Repositories.Abstracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Abstracts
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        //IEnumerable<Comment> GetCommentsByBlogId(int blogId);
        IEnumerable<Comment> GetCommentsByBlogId(int blogId, bool trackChanges, Status? status = null);
        Comment? GetOneComment(int blogId, bool trackChanges, Status? status = null);

        public IQueryable<Comment> GetAllComments(bool trackChanges);

        IEnumerable<Comment> GetCommentsByUserId(string userId, bool trackChanges, Status? status = null);

        void CreateComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(Comment comment);


        public int Count();

    }
}
