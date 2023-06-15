using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace Business.Services.Models
{
    public class TagService : ITagService
    {
        private readonly ITagRepository repository;

        private const string ModifyTagErrorMessage = "Only an admin can modify the tag.";
        private const string ModifyCreateTagErrorMessage = "Only an admin or registered user can create tag.";
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
            bool duplicateExists = false;

            try
            {
                if (loggedUser == null || !loggedUser.IsAdmin)
                {
                    throw new UnauthenticatedOperationException(ModifyCreateTagErrorMessage);
                }

                Tag existingTag = this.repository.GetByName(tagName);
            }

            catch (EntityNotFoundException)
            {
                duplicateExists= true;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"Tag with name '{tagName}' already exists.");
            }

            Tag createdTag = new Tag { Name = tagName };
            
            this.repository.Create(createdTag);

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

    }
}
