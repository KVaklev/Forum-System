using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface IUserService
    {
        List<User> GetAll();
        List<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        User GetByUsername(string username);
        User Promote(User user);
        User BlockUser(User user);
        User UnblockUser(User user);
        User Create(User user);
        User Update(int id, User user, User loggedUser);
        void Delete(int id, User loggedUser);
        bool IsAuthorized(User user, User loggedUser);
        bool CheckIfUsernameExists(string username);
        bool CheckIfEmailExists(string email);
    }
}
