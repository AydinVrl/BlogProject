using Entity.Enums;
using Entity.Models;
using Project.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Abstracts
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAllComments(bool trackChanges, Status? status = null);

        IEnumerable<CommentDTO> GetAllCommentDTOs(bool trackChanges, Status? status = null);

        IEnumerable<Comment> GetAllCommentDTOsByBlogId(bool trackChanges, int blogId, Status? status = null);

        public CommentDTO GetCommentById(int commentId, bool trackChanges);

        public void EditComment(int commentId, CommentDTO updatedComment);
        void CreateComment(CommentDTO commentDTO);
        public void DeleteComment(int commentId);
        int GetCommentCount();
    }
}
