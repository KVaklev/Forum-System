using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;
        public PostService(IPostRepository repository)
        {
            this.repository = repository;
        }
        public List<Post> GetAll()
        {
            return this.repository.GetAll();
        }
        public Post GetById(int id)
        {
<<<<<<< Updated upstream
            return this.repository.GetById(id);
=======
            return this.repository.GetByID(id);
>>>>>>> Stashed changes
        }
        public Post Create(Post post)
        {
            bool duplicateExists = true;

            try
            {
                this.repository.GetByTitle(post.Title);
            }
<<<<<<< Updated upstream
            catch (EntityNotFoundException)
=======
            catch (EntityNotFoundException ex)
>>>>>>> Stashed changes
            {
                duplicateExists = false;
            }
            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Post {post.Title} already exists.");
            }

            Post createdPost = this.repository.Create(post);

            return createdPost;
        }
        public Post Update(int id, Post post)
        {
            bool duplicateExists = true;

            try
            {
                Post existingPost = this.repository.GetByTitle(post.Title);

                if (existingPost.Id == id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Post with title {post.Title} already exists.");
            }

            Post updatedPost = this.repository.Update(id, post);

            return updatedPost;
        }
        public Post Delete(int id)
        {
            return this.repository.Delete(id);
        }
        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }
    }
}
