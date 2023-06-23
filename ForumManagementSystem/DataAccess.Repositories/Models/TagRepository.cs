using DataAccess.Models;
using DataAccess.Repositories.Contracts;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;

namespace DataAccess.Repositories.Models
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationContext context;
        public TagRepository(ApplicationContext context)
        {
           this.context = context;
        }
        public List<Tag> GetAll()
        {
            return context.Tags.ToList();
        }

        public Tag GetById(int id)
        {
            Tag tag = context.Tags.FirstOrDefault(tag => tag.Id == id);
            return tag ?? throw new EntityNotFoundException($"Tag with id {tag.Id} doesn't exist.");
        }

        public Tag GetByName(string name)
        {
            Tag tag = context.Tags.FirstOrDefault(tag => tag.Name == name ) ?? throw new EntityNotFoundException($"Tag with name {name} doesn't exist.");
            return tag;
        }

        public Tag Create(Tag tag)
        {
            context.Tags.Add(tag);
            context.SaveChanges();

            return tag;
        }

        public Tag Delete(int id)
        {
            Tag tagToDelete = this.GetById(id);
            context.Tags.Remove(tagToDelete);
            context.SaveChanges();

            return tagToDelete;
        }

        public Tag Edit(int id, Tag tag)
        {
            Tag tagToEdit = this.GetById(id);
            tagToEdit.Name = tag.Name;
            context.SaveChanges();

            return tagToEdit;
        }

        public bool NameExists(string name)
        {
            return context.Tags.Any(u => u.Name == name);
        }
    }
}