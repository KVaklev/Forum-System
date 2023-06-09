using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        List<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        User GetByUsername(string username);
        User GetByEmail(string email);
        User GetByFirstName(string firstName);
        User Promote (User user);
        User Create(User user);
        User Update(int id, User user);
        User Delete(int id);
    }
}
