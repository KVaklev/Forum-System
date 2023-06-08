using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface IUserService
    {
        List<User> GetAll();
        List<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        User Create(User user);
        User Update(int id, User user);
        User Delete(int id);
    }
}
