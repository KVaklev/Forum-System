using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class UserService : IUserService
    {
        private const string ModifyUserErrorMessage = "Only owner or admin can modify a user.";
        private const string ModifyUsernameErrorMessage = "Username change is not allowed.";
        private readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public List<User> GetAll()
        {
            return this.repository.GetAll();
        }
        public User GetById(int id)
        {
           return this.repository.GetById(id);
        }
        public User GetByUsername(string username)
        {
            return this.repository.GetByUsername(username);
        }
        public List<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }
        public User Create(User user)
        {
            if (this.repository.UsernameExists(user.Username))
            {
                throw new DuplicateEntityException($"User with username '{user.Username}' already exists.");
            }

            if (this.repository.EmailExist(user.Email))
            {
                throw new DuplicateEntityException($"User with email '{user.Email}' already exists.");
            }

            User createdUser = this.repository.Create(user);

            return createdUser;
            
        }
        public User Update(int id, User user, User loggedUser)
        {
            User userToUpdate = this.repository.GetById(id);

            if (!IsAuthorized(userToUpdate, loggedUser))
            {
                throw new UnauthorizedOperationException(ModifyUserErrorMessage);
            }

            if (user.Username!=null)
            {
                throw new InvalidOperationException(ModifyUsernameErrorMessage);
            }

            if (this.repository.EmailExist(user.Email))
            {
                throw new DuplicateEntityException($"User with email '{user.Email}' already exists.");
            }

            User updatedUser = this.repository.Update(id, user);

            return updatedUser;
        }
        private bool IsAuthorized(User user, User loggedUser)
        {
            bool isAuthorized = false;

            if (user.Equals(loggedUser) || loggedUser.IsAdmin)
            {
                isAuthorized = true;
            }
            return isAuthorized;
        }
        public void Delete(int id, User loggedUser)
        {
            User user = repository.GetById(id);

            if (!IsAuthorized(user, loggedUser))
            {
                throw new UnauthorizedOperationException(ModifyUserErrorMessage);
            }
            this.repository.Delete(id);
        }
        public User Promote(User user)
        {
            return this.repository.Promote(user);
        }
        public User BlockUser(User user)
        {
            return this.repository.BlockUser(user);
        }
        public User UnblockUser(User user)
        {
            return this.repository.UnblockUser(user);
        }
    }
}
