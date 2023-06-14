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
    public class TagRepository : ITagRepository
    {
        private readonly List<Tag> tags;
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
            Tag tag = context.Tags.FirstOrDefault(tag => tag.Name == name);
            return tag ?? throw new EntityNotFoundException($"Tag with name {tag.Id} doesn't exist.");
        }

        public Tag Create(Tag tag, User loggedUser)
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

    }
}