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
        User BlockUser (User user);
        User UnblockUser (User user);
        User Create(User user);
        User Update(int id, User user, User loggedUser);
        User Delete(int id);
		void UpdatePhoneNumber(User user, User userToUpdate, User loggedUser);
        bool UsernameExists(string username);
        bool EmailExists(string email);

	}
}
