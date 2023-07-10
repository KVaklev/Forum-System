using AspNetCoreDemo.Models;
using Business.Exceptions;
using Business.Services.Helpers;
using DataAccess.Repositories.Contracts;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Repository;

namespace ForumManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly ILikeCommentRepository likeRepository;
        public UserService(IUserRepository repository, ILikeCommentRepository likeRepository)
        {
            this.repository = repository;
            this.likeRepository = likeRepository;
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
        public PaginatedList<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this.repository.FilterBy(filterParameters);
        }
        public User Create(User user)
        {
            if (UsernameExists(user.Username))
            {
                throw new DuplicateEntityException($"User with username '{user.Username}' already exists.");
            }

            if (EmailExists(user.Email))
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
                throw new UnauthorizedOperationException(Constants.ModifyUserErrorMessage);
            }

            if (user.Email!=userToUpdate.Email)
            {
				if (EmailExists(user.Email))
				{
					throw new DuplicateEntityException($"User with email '{user.Email}' already exists.");
				}
			}

			userToUpdate = this.repository.Update(id, user, loggedUser);
			return userToUpdate;
		}

        public void Delete(int id, User loggedUser)
        {
            User user = repository.GetById(id);

            if (!IsAuthorized(user, loggedUser))
            {
                throw new UnauthorizedOperationException(Constants.ModifyUserErrorMessage);
            }
            likeRepository.DeletedByUser(user);
            this.repository.Delete(id);
        }

        public bool IsAuthorized(User user, User loggedUser)
        {
            bool isAuthorized = false;

            if (user.Id == loggedUser.Id || loggedUser.IsAdmin)
            {
                isAuthorized = true;
            }
            return isAuthorized;
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

        public bool UsernameExists(string username)
        {
            return this.repository.UsernameExists(username);
        }

        public bool EmailExists(string email)
        {
            return this.repository.EmailExists(email);
        }
    }
}
