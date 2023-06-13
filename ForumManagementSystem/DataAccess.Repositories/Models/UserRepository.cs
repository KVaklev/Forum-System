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
<<<<<<< HEAD
                     Password = "MTIz",//123
=======
                     Password = "MTIz", //123
>>>>>>> 5d8078097ebdca94a3531d81e59f5767a63c4038
                     IsAdmin = true,
                 },
                 new User()
                 {
                     Id = 2,
                     FirstName = "Mariq",
                     LastName = "Petrova",
                     Email = "m.petrova@gmail.com",
                     Username = "mariicheto",
<<<<<<< HEAD
                     Password = "@sfjsddawdljsl",//QHNmanNkZGF3ZGxqc2w=
                     IsAdmin = false,
                     IsBlocked = true
=======
                     Password = "QHNmanNkZGF3ZGxqc2w=", //@sfjsddawdljsl
>>>>>>> 5d8078097ebdca94a3531d81e59f5767a63c4038
                 }
            };
        }
        public List<User> GetAll()
        {
           return this.users;
        }
        public User GetById(int id)
        {
           User user=this.users.Where(users => users.Id == id).FirstOrDefault();
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
        public User Update(int id, User user)
        {
            User userToUpdate = this.GetById(id);

            userToUpdate.FirstName = user.FirstName ?? userToUpdate.FirstName;
            userToUpdate.LastName = user.LastName ?? userToUpdate.LastName;
            userToUpdate.Password = user.Password ?? userToUpdate.Password;
            userToUpdate.IsAdmin = user.IsAdmin;

            UpdatePhoneNumber(user, userToUpdate);

            return userToUpdate;
        }

        public void UpdatePhoneNumber(User user, User userToUpdate)
        {

            if (user.IsAdmin)
            {
                if (userToUpdate.PhoneNumber == null)
                {
                    userToUpdate.PhoneNumber = user.PhoneNumber;
                }
            }
            else
            {
                if (user.PhoneNumber != null)
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
