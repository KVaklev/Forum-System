using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Models
{
    public class LikeCommentService : ILikeCommentService
    {
        private const string ModifyLikeErrorMessage = "Only the correct user can chages IsLiked.";
        private readonly ILikeCommentRepository repository;

        public LikeCommentService(ILikeCommentRepository repository)
        {
            this.repository = repository;
        }
        
        public LikeComment Delete(Comment comment, User user)
        { 
           var likeCommentToDelete = this.repository.Get(comment,user);
           return likeCommentToDelete;
        }

        public LikeComment Get(Comment comment, User user)
        {
            return this.repository.Get(comment, user);
        }

        public LikeComment Update(Comment comment, User user)
        {
            try
            {
            var likeCommentToUpdate = this.repository.Get(comment, user);
            }
            catch (EntityNotFoundException)
            {
                return this.repository.Create(comment, user);
            }
            return this.repository.Update(comment, user);
        }
    }
}
