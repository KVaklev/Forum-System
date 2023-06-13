using DataAccess.Models;
using DataAccess.Repositories.Contracts;
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

        public TagRepository()
        {
            this.tags = new List<Tag>()
            {
                new Tag()
                {
                    Id= 1,
                    PostId = 1,
                    Name = "Bmw"
                },
                new Tag()
                {
                    Id= 2,
                    PostId = 1,
                    Name = "Fiat",
                },
                 new Tag()
                {
                    Id= 3,
                    PostId = 2,
                    Name = "Toyota",
                }
            };
        }
        public Tag Create(Tag tag)
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
    }

    
}