using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using DataAccess.Repositories.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace Business.Services.Models
{
    public class TagService : ITagService
    {
        private readonly ITagRepository repository;
        private const string ModifyTagErrorMessage = "Only an admin or user created the tag can modify the tag.";
        private const string ModifyCreateTagErrorMessage = "Only an admin can delete tag.";
        private const string AddToPostErrorMessage = "Only the post owner or admins can add tags to the post.";

        private readonly ITagRepository tagRepository;
        private readonly IPostRepository postRepository;
        public TagService(ITagRepository tagRepository, IPostRepository postRepository)
        {
            this.tagRepository = tagRepository;
            this.postRepository = postRepository;
        }
        public List<Tag> GetAll()
        {
            return this.repository.GetAll();
        }
        public Tag GetById(int id)
        {
            return this.repository.GetById(id);
        }
        public Tag GetByName(string name)
        {
            return this.repository.GetByName(name);
        }
        public Tag Create(Tag tag, User loggedUser)
        {
            bool duplicateExists = false;

            try
            {
                if (loggedUser == null || !loggedUser.IsAdmin)
                {
                    throw new UnauthenticatedOperationException(ModifyCreateTagErrorMessage);
                }

                Tag existingTag = this.repository.GetByName(tag.Name);
            }

            catch (EntityNotFoundException)
            {
                duplicateExists= true;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Tag with name '{tag.Name}' already exists.");
            }

            Tag createdTag = this.repository.Create(tag, loggedUser);

            return createdTag;

        }
        public void Delete(int id, User loggedUser)
        {
            Tag tag = repository.GetById(id);

            if (!loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyTagErrorMessage);
            }

            this.repository.Delete(id);
        }

        public Tag Edit(int id, Tag tag, User loggedUser)
        {
            Tag tagToEdit = repository.GetById(id);

            if (!loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyTagErrorMessage);
            }
            bool duplicateExists = false;

            try
            {
                this.repository.GetByName(tag.Name);
            }
            catch (EntityNotFoundException)

            {
                duplicateExists = true;
            }
            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Tag with name '{tag.Name}' already exists.");
            }

            Tag editedTag = this.repository.Edit(id, tag);

            return editedTag;
        }

        public void AddTagToPost(int postId, User loggedUser)
        {
            Post post=this.postRepository.GetById(postId);

            if (post.UserId != loggedUser.Id || !loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(AddToPostErrorMessage);
            }
        }

        public void RemoveTagFromPost(int postId, User loggedUser)
        {
            throw new NotImplementedException();
        }
    }
}
