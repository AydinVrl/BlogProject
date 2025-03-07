using Project.Repositories.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.UnitOfWork
{
    public interface IRepositoryManager
    {
        IBlogRepository BlogRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }

        void Save();
    }
}
