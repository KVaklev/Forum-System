using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class UserService : IUserService
    {
        private const string ModifyUserErrorMessage = "Only owner or admin can modify a user.";
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
        public User Create(User user)
        {
            bool duplicateExists = true;

            try
            {
                this.repository.GetByUsername(user.Username);
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }
            if (duplicateExists)
            {
                throw new DuplicateEntityException($"User with username {user.Username} already exists.");
            }
            try
            {
                this.repository.GetByEmail(user.Email);
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }
            if (duplicateExists)
            {
                throw new DuplicateEntityException($"User with email {user.Email} already exists.");
            }

            User createdUser=this.repository.Create(user);

            return createdUser;
        }
        public User Update(int id, User user, User loggedUser)
        {
            User userToUpdate = this.repository.GetById(id);

            if (!userToUpdate.Equals(loggedUser) && !loggedUser.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyUserErrorMessage);
            }
            bool duplicateExists = true;

            try
            {
                User existingUser = this.repository.GetByUsername(userToUpdate.Username);

                if (existingUser.Id == id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"User with username '{user.Username}' already exists.");
            }
            try
            {
                User existingUser = this.repository.GetByEmail(userToUpdate.Email);

                if (existingUser.Id == id)
                {
                    duplicateExists = false;
                }
            }
            catch (EntityNotFoundException)
            {
                duplicateExists = false;
            }

            if (duplicateExists)
            {
                throw new DuplicateEntityException($"User with email '{user.Email}' already exists.");
            }


            User updatedUser = this.repository.Update(id, user);

            return updatedUser;
        }
     
        public List<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }

        public User GetByUserName(string username)
        {
            return this.repository.GetByUsername(username);
        }

        public User Promote(User user)
        {
            return this.repository.Promote(user);
        }

        public void Delete(int id, User loggedUser)
        {
            User user = repository.GetById(id);
            if (!user.Equals(user) && !user.IsAdmin)
            {
                throw new UnauthorizedOperationException(ModifyUserErrorMessage);
            }

            this.repository.Delete(id);
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
