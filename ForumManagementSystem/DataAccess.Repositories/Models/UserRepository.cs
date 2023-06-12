using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> users;

        private const string ModifyUserErrorMessage = "Only admin can add a phone number.";

        public UserRepository()
        {
            this.users = new List<User>()
            {
                     new User()
                 {
                     Id = 1,
                     FirstName = "Ivan",
                     LastName = "Draganov",
                     Email = "i.draganov@gmail.com",
                     Username = "ivanchoDraganchov",
                     Password = "MTIz",
                     IsAdmin = true,
                 },
                 new User()
                 {
                     Id = 2,
                     FirstName = "Mariq",
                     LastName = "Petrova",
                     Email = "m.petrova@gmail.com",
                     Username = "mariicheto",
                     Password = "@sfjsddawdljsl",
                 }
            };
        }
        public List<User> GetAll()
        {
           return this.users;
        }
        public User GetById(int id)
        {
           User? user=this.users.Where(users => users.Id == id).FirstOrDefault();
           return user ?? throw new EntityNotFoundException($"User with ID = {id} doesn't exist.");
        }
        public User GetByUsername(string username)
        {
            User user = this.users.Where(users => users.Username == username).FirstOrDefault();

            return user ?? throw new EntityNotFoundException($"User with username '{username}' doesn't exist.");
        }
        public User GetByEmail(string email)
        {
            User user = this.users.Where(users => users.Email == email).FirstOrDefault();

            return user ?? throw new EntityNotFoundException($"User with email '{email}' doesn't exist.");
        }
        public User GetByFirstName(string firstName)
        {
            User user = this.users.Where(users => users.FirstName == firstName).FirstOrDefault();

            return user ?? throw new EntityNotFoundException($"User with first name '{firstName}' doesn't exist.");
        }
        public User Create(User user)
        {
            user.Id = this.users.Count + 1;
            this.users.Add(user);
            return user;
        }
        public User Delete(int id)
        {
            User userToDelete = this.GetById(id);
            this.users.Remove(userToDelete);
            return userToDelete;
        }
        public User Update(int id, User loggedUser)
        {
            User userToUpdate = this.GetById(id);

            userToUpdate.FirstName = loggedUser.FirstName ?? userToUpdate.FirstName;
            userToUpdate.LastName = loggedUser.LastName ?? userToUpdate.LastName;
            userToUpdate.Email = loggedUser.Email ?? userToUpdate.Email;
            userToUpdate.Password = loggedUser.Password ?? userToUpdate.Password;

            UpdatePhoneNumber(loggedUser, userToUpdate);

            return userToUpdate;
        }

        public void UpdatePhoneNumber(User loggedUser, User userToUpdate)
        {
            if (loggedUser.IsAdmin)
            {
                if (loggedUser.PhoneNumber != null)
                {
                    userToUpdate.PhoneNumber = loggedUser.PhoneNumber;
                }
            }
            else
            {
                if (loggedUser.PhoneNumber != null)
                {
                    throw new UnauthorizedOperationException(ModifyUserErrorMessage);
                }
            }
        }

        public List<User> FilterBy(UserQueryParameters filterParameters)
        {
            List<User> result = this.users;

            //Search by name, email, username, posts

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

            return result;
        }

        public User Promote(User user)
        {
            if (!user.IsAdmin)
            {
                user.IsAdmin = true;
            }
            return user;
        }

        public User BlockUser(User user)
        {
            if (!user.IsBlocked)
            {
                user.IsBlocked = true;
            }
            return user;
        }

        public User UnblockUser(User user)
        {
            if (user.IsBlocked)
            {
                user.IsBlocked = false;
            }
            return user;
        }
    }
}
