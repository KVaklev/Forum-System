using DataAccess.Models;
using ForumManagementSystem.Models;

namespace Business.Services.Contracts
{
    public interface ITagService
    {
        List<Tag> GetAll();

        Tag GetById(int id);

        Tag GetByName(string name);

        Tag Create(Tag tag, User loggedUser);

        Tag Edit(int id, Tag tag, User loggedUsed);

        void Delete(int id, User loggedUser);

        void AddTagToPost(int postId, User loggedUser);

        void RemoveTagFromPost(int postId, User loggedUser);
    }
}
