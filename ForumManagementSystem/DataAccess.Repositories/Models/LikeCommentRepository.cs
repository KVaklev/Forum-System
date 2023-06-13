using DataAccess.Models;
using DataAccess.Repositories.Contracts;
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
        private readonly List<LikeComment> likes;
        public LikeCommentRepository() 
        {
            this.likes = new List<LikeComment>();
        }

        public LikeComment Get(Comment comment, User user)
        {
            var likeComment = this.likes
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
            likeComment.Id = this.likes.Count + 1;
            likeComment.UserId=user.Id;
            likeComment.CommentId = comment.Id;
            likeComment.IsLiked = true;
            likes.Add(likeComment);
            return likeComment;
        }

        public LikeComment Delete(Comment comment, User user)
        {
            var likeToDeleted = Get(comment,user);
            likes.Remove(likeToDeleted);
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
            return likeCommentToUpdate;
        }
    }
}
