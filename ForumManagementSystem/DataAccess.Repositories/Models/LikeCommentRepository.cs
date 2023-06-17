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
            comment.LikesCount++;
            context.SaveChanges();
            return likeComment;
        }
        //TODO - foreach likes with comentId. 
        
        public int DeleteByComment(Comment comment)
        {
            List<LikeComment> likesToDeleted = context.LikeComments
                .Where(u => u.CommentId == comment.Id)
                .ToList();
            int countToDeleted = 0;

            foreach (var likes in likesToDeleted)
            {
                context.LikeComments.Remove(likes);
            }
            context.SaveChanges();
            return countToDeleted;
        }
        // TODO - to change the bool isDeleted;
        public bool DeletedByUser(User deletedUser)
        {
            bool isDeleted = false;
            List<LikeComment> deletedUsersLike = this.context.LikeComments
                .Where(u => u.UserId == deletedUser.Id)
                .ToList();
            foreach (var likeComment in deletedUsersLike)
            {
                if (likeComment.IsLiked)
                {
                    isDeleted = true;
                    Comment comment = this.context.Comments
                           .Where(c => c.Id == likeComment.CommentId)
                           .FirstOrDefault();
                    comment.LikesCount--;
                }
            }
            context.SaveChanges();
            return isDeleted;
        }

        public LikeComment Update(Comment comment, User user)
        {
            var likeCommentToUpdate = Get(comment,user);
            if (likeCommentToUpdate.IsLiked)
            {
                likeCommentToUpdate.IsLiked = false;
                comment.LikesCount--;
            }
            else
            {
                likeCommentToUpdate.IsLiked = true;
                comment.LikesCount++;
            }
            context.SaveChanges();
            return likeCommentToUpdate;
        }
    }
}
