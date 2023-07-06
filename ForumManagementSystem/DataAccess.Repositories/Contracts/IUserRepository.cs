using AspNetCoreDemo.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        PaginatedList<User> FilterBy(UserQueryParameters filterParameters);
        List<User> Paginate(List<User> result, int pageNumber, int pageSize);
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
