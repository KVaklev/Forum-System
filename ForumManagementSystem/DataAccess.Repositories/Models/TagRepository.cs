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
            return this.tags;
        }

        public Tag GetById(int id)
        {
            Tag tag = this.tags.FirstOrDefault(tag => tag.Id == id);
            return tag ?? throw new EntityNotFoundException($"Tag with id {tag.Id} doesn't exist.");
        }

        public Tag GetByName(string name)
        {
            Tag tag = this.tags.FirstOrDefault(tag => tag.Name == name);
            return tag ?? throw new EntityNotFoundException($"Tag with name {tag.Id} doesn't exist.");
        }

        public Tag Create(Tag tag, User loggedUser)
        {
            tag.Id = this.tags.Count + 1;
            this.tags.Add(tag);
            return tag;
        }

        public Tag Delete(int id)
        {
            Tag tagToDelete = this.GetById(id);
            this.tags.Remove(tagToDelete);
            return tagToDelete;
        }

        public Tag Edit(int id, Tag tag)
        {
            Tag tagToEdit = this.GetById(id);

            tagToEdit.Name = tag.Name;

            return tagToEdit;
        }

        public void AddTagToPost(int postId, User loggedUser)
        {
            throw new NotImplementedException();
        }
        public void RemoveTagFromPost(int postId, User loggedUser)
        {
            throw new NotImplementedException();
        }

    }
}