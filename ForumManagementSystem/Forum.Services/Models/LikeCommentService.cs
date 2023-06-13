using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
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
        public LikeComment Create(Comment comment, User user)
        {
            return this.repository.Create(comment, user);
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
            var likeCommentToUpdate = this.repository.Get(comment, user);
            
            if (likeCommentToUpdate.UserId != user.Id)
            {
                throw new UnauthorizedOperationException(ModifyLikeErrorMessage);
            }
            
            return this.repository.Update(comment, user);
        }
    }
}
