using Business.Services.Contracts;
using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Models;

namespace Business.Services.Models
{
    public class TagService : ITagService
    {
        private readonly ITagRepository repository;

        public TagService(ITagRepository repository)
        {
            this.repository = repository;
        }
        public Tag Create(Tag tag, Post post)
        {
            throw new NotImplementedException();
        }

        public Tag Delete(int id)
        {
            throw new NotImplementedException();
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
