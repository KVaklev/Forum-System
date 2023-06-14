using DataAccess.Models;
using ForumManagementSystem.Models;

namespace DataAccess.Repositories.Contracts
{
    public  interface ITagRepository
    {
        List<Tag> GetAll();

        Tag GetById(int id);

        Tag GetByName(string name);

        Tag Create(Tag tag, User loggedUser);

        Tag Edit(int id, Tag tag);

        Tag Delete(int id);

        void AddTagToPost(int postId, User loggedUser);

        void RemoveTagFromPost(int postId, User loggedUser);

    }
}
