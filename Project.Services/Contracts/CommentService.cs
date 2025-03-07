using AutoMapper;
using Entity.Enums;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Project.Entities.DTOs;
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
    public class CommentService : ICommentService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public CommentService(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        public CommentDTO GetCommentById(int commentId, bool trackChanges)
        {
            var comment = _manager.CommentRepository
                                  .GetAllComments(trackChanges)
                                  .FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
                throw new Exception("Comment not found");

            return _mapper.Map<CommentDTO>(comment);
        }
        public void CreateComment(CommentDTO commentDTO)
        {
            var comment = new Comment() { BlogId=commentDTO.BlogId, Description=commentDTO.Description,UserId=commentDTO.UserId };
            //var oneBlog = _mapper.Map<Blog>(blog);
            _manager.CommentRepository.Create(comment);
            _manager.Save();
        }
        public void EditComment(int commentId, CommentDTO updatedComment)
        {
            var existingComment = _manager.CommentRepository
                                          .GetAllComments(true)
                                          .FirstOrDefault(c => c.Id == commentId);
            if (existingComment == null)
                throw new Exception("Comment not found");

            existingComment.Description = updatedComment.Description;
            existingComment.UserId = updatedComment.UserId;
            existingComment.BlogId = updatedComment.BlogId;
            existingComment.Checked = updatedComment.Checked;

            _manager.CommentRepository.Update(existingComment);
            _manager.Save();
        }
        public void DeleteComment(int commentId)
        {
            var comment = _manager.CommentRepository
                                  .GetAllComments(true)
                                  .FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                throw new Exception("Comment not found");

            _manager.CommentRepository.Delete(comment);
            _manager.Save();
        }

        public IEnumerable<CommentDTO> GetAllCommentDTOs(bool trackChanges, Status? status = null)
        {
            _mapper.Map<IEnumerable<CommentDTO>>(_manager.BlogRepository.GetAllBlogs(true));

            if (status is null)
                return _mapper.Map<IEnumerable<CommentDTO>>(_manager.CommentRepository.GetAllComments(true));
            else
                return _mapper.Map<IEnumerable<CommentDTO>>(_manager.CommentRepository.GetAllComments(true).StatusActive(status));

        }
 
       
        public IEnumerable<Comment> GetAllCommentDTOsByBlogId(bool trackChanges, int blogId, Status? status = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAllComments(bool trackChanges, Status? status = null)
        {
            throw new NotImplementedException();
        }

        public int GetCommentCount()
        {
            return _manager.CommentRepository.Count();
        }


    }
}
