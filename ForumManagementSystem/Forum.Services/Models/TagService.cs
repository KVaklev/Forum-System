using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using DataAccess.Repositories.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace Business.Services.Models
{
    public class TagService : ITagService
    {
        private readonly ITagRepository repository;
        private const string ModifyPostErrorMessage = "Only an admin or user created the tag can modify the tag.";

        public TagService(ITagRepository repository)
        {
            this.repository = repository;
        }
        public Tag Create(Tag tag)
        {
            try
            {
                Tag existingTag = this.repository.GetByName(tag.Name);
            }
            catch (EntityNotFoundException)
            {
                Tag newTag= this.repository.Create(tag);

                return newTag;
            }

            throw new DuplicateEntityException($"Tag with name '{tag.Name}' already exists.");

        }

        public void Delete(int id, User loggedUser)
        {
            Tag tag = repository.GetById(id);

            if (!loggedUser.Equals(tag.CreatedBy) && !loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyPostErrorMessage);
            }

            this.repository.Delete(id);
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
    }
}
