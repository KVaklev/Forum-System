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
    public class LikePostService : ILikePostService
    {
        private readonly ILikePostRepository repository;

        public LikePostService(ILikePostRepository repository)
        {
            this.repository = repository;
        }
        public LikePost Delete(Post post, User user)
        {
            var likePostToBeDeleted = this.repository.Get(post, user);
            return likePostToBeDeleted;
        }

        public LikePost Update(Post post, User user)
        {
            try
            {
                var likePostToBeUpdated = this.repository.Get(post, user);
            }
            catch(EntityNotFoundException)
            {
                return this.repository.Create(post, user);
            }

            return this.repository.Update(post, user);
        }
    }
}
