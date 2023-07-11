using AspNetCoreDemo.Models;
using Business.Exceptions;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ForumManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string ModifyUserErrorMessage = "Only admin can add a phone number.";

        private readonly ApplicationContext context;
        public UserRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public User GetById(int id)
        {
            User user = context.Users.Where(users => users.Id == id).FirstOrDefault();
            return user ?? throw new EntityNotFoundException($"User with ID = {id} doesn't exist.");
        }

        public User GetByUsername(string username)
        {
            User user = context.Users.Where(users => users.Username == username).FirstOrDefault();

            return user ?? throw new EntityNotFoundException($"User with username '{username}' doesn't exist.");
        }

        public User GetByEmail(string email)
        {
            User user = context.Users.Where(users => users.Email == email).FirstOrDefault();

            return user ?? throw new EntityNotFoundException($"User with email '{email}' doesn't exist.");
        }

        public User GetByFirstName(string firstName)
        {
            User user = context.Users.Where(users => users.FirstName == firstName).FirstOrDefault();

            return user ?? throw new EntityNotFoundException($"User with first name '{firstName}' doesn't exist.");
        }

        public User Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public User Delete(int id)
        {
            User userToDelete = this.GetById(id);
            userToDelete=context.Users
                .Include(u=>u.Comments)
                .FirstOrDefault(u => u.Id == id);
            context.Comments.RemoveRange(userToDelete.Comments);
            context.Users.Remove(userToDelete);
            context.SaveChanges();

            return userToDelete;
        }

        public User Update(int id, User user, User loggedUser)
		{
			User userToUpdate = this.GetById(id);

			userToUpdate.FirstName = user.FirstName ?? userToUpdate.FirstName;
			userToUpdate.LastName = user.LastName ?? userToUpdate.LastName;
			userToUpdate.Password = user.Password ?? userToUpdate.Password;
			userToUpdate.Address = user.Address ?? userToUpdate.Address;
			userToUpdate.Country = user.Country ?? userToUpdate.Country;
			userToUpdate.DateOfBirth = user.DateOfBirth ?? userToUpdate.DateOfBirth;
			userToUpdate.Email = user.Email ?? userToUpdate.Email;
			userToUpdate.Username = user.Username ?? userToUpdate.Username;

			UpdateAdminStatus(user, userToUpdate);

			userToUpdate.IsBlocked = user.IsBlocked;

			UpdatePhoneNumber(user, userToUpdate, loggedUser);

			context.SaveChanges();

			return userToUpdate;
		}

		public static void UpdateAdminStatus(User user, User userToUpdate)
		{
			if (!userToUpdate.IsAdmin)
			{
				userToUpdate.IsAdmin = user.IsAdmin;
			}
			else
			{
				userToUpdate.IsAdmin = true;
			}
		}

		public void UpdatePhoneNumber(User user, User userToUpdate, User loggedUser)
        {
            if (user.IsAdmin || loggedUser.IsAdmin)
            {
              userToUpdate.PhoneNumber = user.PhoneNumber ?? userToUpdate.PhoneNumber;
            }
            else
            {
                if (user.PhoneNumber != null)
                {
                    throw new UnauthorizedOperationException(ModifyUserErrorMessage);
                }
            }
        }

        public PaginatedList<User> FilterBy(UserQueryParameters filterParameters)
        {
            List<User> result = context.Users.ToList();

            if (!string.IsNullOrEmpty(filterParameters.FirstName))
            {
                result = result.FindAll(user => user.FirstName.Contains(filterParameters.FirstName));
            }
            if (!string.IsNullOrEmpty(filterParameters.LastName))
            {
                result = result.FindAll(user => user.LastName.Contains(filterParameters.LastName));
            }
            if (!string.IsNullOrEmpty(filterParameters.Username))
            {
                result = result.FindAll(user => user.Username.Contains(filterParameters.Username));
            }
            if (!string.IsNullOrEmpty(filterParameters.Email))
            {
                result = result.FindAll(user => user.Email.Contains(filterParameters.Email));
            }
            if (filterParameters.Admin.HasValue)
            {
                result = result.FindAll(user => user.IsAdmin == filterParameters.Admin.Value);
            }

            if (filterParameters.Blocked.HasValue)
            {
                result = result.FindAll(user => user.IsBlocked == filterParameters.Blocked.Value);
            }


            if (!string.IsNullOrEmpty(filterParameters.SortBy))
            {
                if (filterParameters.SortBy.Equals("firstName", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(user => user.FirstName).ToList();
                }
                else if (filterParameters.SortBy.Equals("lastName", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(user => user.LastName).ToList();
                }

                if (!string.IsNullOrEmpty(filterParameters.SortOrder) && filterParameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }

            int totalPages = ((result.Count() + 1) / filterParameters.PageSize)+1;
            
            result = Paginate(result, filterParameters.PageNumber, filterParameters.PageSize);

            return new PaginatedList<User>(result, totalPages, filterParameters.PageNumber);
        }

        public List<User> Paginate(List<User> result, int pageNumber, int pageSize)
        {
            return result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
        }

        public User Promote(User user)
        {
            if (!user.IsAdmin)
            {
                user.IsAdmin = true;
            }
            context.SaveChanges();

            return user;
        }

        public User BlockUser(User user)
        {
            if (!user.IsBlocked)
            {
                user.IsBlocked = true;
            }
            context.SaveChanges();

            return user;
        }

        public User UnblockUser(User user)
        {
            if (user.IsBlocked)
            {
                user.IsBlocked = false;
            }
            context.SaveChanges();

            return user;
        }

        public bool UsernameExists(string username)
        {
            return context.Users.Any(u => u.Username == username);
        }

        public bool EmailExists(string email)
        {
            return context.Users.Any (u => u.Email == email);
        }
    }
}
