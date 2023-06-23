using Business.Exceptions;
using Business.Services.Contracts;
using Business.Services.Helpers;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace Business.Services.Models
{
    public class TagService : ITagService
    {
        private readonly ITagRepository repository;

        public TagService(ITagRepository repository)
        {
           this.repository=repository;
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
        public Tag Create(string tagName, User loggedUser)
        {

            if (loggedUser == null || !loggedUser.IsAdmin)
            {
                throw new UnauthenticatedOperationException(Constants.ModifyCreateTagErrorMessage);
            }

            try
            {
                Tag existingTag = this.repository.GetByName(tagName);

                return existingTag;
            }
            catch (EntityNotFoundException)
            {
                Tag createdTag = new Tag { Name = tagName };

                this.repository.Create(createdTag);

                return createdTag;
            }
        }
        public void Delete(int id, User loggedUser)
        {
            Tag tag = repository.GetById(id);

            if (!loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(Constants.ModifyTagErrorMessage);
            }

            this.repository.Delete(id);
        }

        public Tag Edit(int id, Tag tag, User loggedUser)
        {
            Tag tagToEdit = repository.GetById(id);

            if (!loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(Constants.ModifyTagErrorMessage);
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

    }
}
