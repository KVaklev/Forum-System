using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Models
{
    public class LikeCommentRepository : ILikeCommentRepository
    {
        private readonly ApplicationContext context;
        public LikeCommentRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public LikeComment Get(Comment comment, User user)
        {
            LikeComment likeComment = this.context.LikeComments
                .Where(u => u.UserId == user.Id)
                .Where(c => c.CommentId == comment.Id)
                .FirstOrDefault();

            if (likeComment==null)
            {
                throw new EntityNotFoundException($"This comment is not liked from this user.");
            }
            return likeComment;
        }

        public LikeComment Create(Comment comment, User user)
        {
            LikeComment likeComment = new LikeComment();
         
            likeComment.UserId=user.Id;
            likeComment.CommentId = comment.Id;
            likeComment.IsLiked = true;
            context.SaveChanges();
            return likeComment;
        }
        //TODO - foreach likes with comentId. 
        public LikeComment Delete(Comment comment, User user)
        {
            var likeToDeleted = Get(comment,user);
            context.LikeComments.Remove(likeToDeleted);
            context.SaveChanges();
            return likeToDeleted;
        }

        public LikeComment Update(Comment comment, User user)
        {
            var likeCommentToUpdate = Get(comment,user);
            if (likeCommentToUpdate.IsLiked)
            {
                likeCommentToUpdate.IsLiked = false;
            }
            else
            {
                likeCommentToUpdate.IsLiked = true;
            }
            context.SaveChanges();
            return likeCommentToUpdate;
        }
    }
}
