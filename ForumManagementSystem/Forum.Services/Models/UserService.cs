using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class UserService : IUserService
    {
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
                throw new DuplicateEntityException($"Username {user.Username} already exists.");
            }
            User createdUser=this.repository.Create(user);

            return createdUser;
        }
        public User Update(int id, User user)
        {
            bool duplicateExists = true;

            try
            {
                User existingUser = this.repository.GetById(id);

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
                throw new DuplicateEntityException($"User with id '{user.Id}' already exists.");
            }

            User updatedUser = this.repository.Update(id, user);

            return updatedUser;
        }
        public User Delete(int id)
        {
            return this.repository.Delete(id);
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
    }
}
