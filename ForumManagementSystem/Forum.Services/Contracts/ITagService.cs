using DataAccess.Models;
using ForumManagementSystem.Models;

namespace Business.Services.Contracts
{
    public interface ITagService
    {
        List<Tag> GetAll();

        Tag GetById(int id);

        Tag GetByName(string name);

        Tag Create(Tag tag, Post post);

        Tag Delete(int id);
    }
}
