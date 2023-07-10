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
    public class LikePostRepository : ILikePostRepository
    {
        private readonly ApplicationContext context;

        public LikePostRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public LikePost Get(Post post, User user)
        {
            LikePost likePost = this.context.LikePosts
                .Where(u => u.UserId == user.Id)
                .Where(p => p.PostId == post.Id)
                .FirstOrDefault();

            if (likePost == null)
            {
                throw new EntityNotFoundException($"Post with title {post.Title}  is not liked by {user.Username}.");
            }
            return likePost;
        }
        public LikePost Create(Post post, User user)
        {
            LikePost likePost = new LikePost();

            likePost.UserId = user.Id;
            likePost.PostId = post.Id;
            likePost.IsLikedPost = true;
            post.PostLikesCount++;
            context.LikePosts.Add(likePost);
            context.SaveChanges();
            return likePost;
        }

        public LikePost Update(Post post, User user)
        {
            var postLikeToBeUpdated = Get(post, user);

            if (postLikeToBeUpdated.IsLikedPost)
            {
                postLikeToBeUpdated.IsLikedPost = false;
                post.PostLikesCount--;
            }
            else
            {
                postLikeToBeUpdated.IsLikedPost = true;
                post.PostLikesCount++;
            }

            context.SaveChanges();
            return postLikeToBeUpdated;
        }

        public LikePost Delete(Post post, User user)
        {
            var postLikeToBeDeleted = Get(post, user);
            context.LikePosts.Remove(postLikeToBeDeleted);
            context.SaveChanges();
            return postLikeToBeDeleted;
        }




    }
}
