using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using System.Text;

namespace Presentation.Helpers
{
    public class AuthManager
    {
        private readonly IUserService userService;

        public AuthManager(IUserService userService)
        {
            this.userService = userService;   
        }

        public User TryGetUser(string credentials)
        {
            string[] credentialsArray = credentials.Split(':');
            string username = credentialsArray[0];
            string password = credentialsArray[1];

            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            try
            {
                var user = this.userService.GetByUserName(username);
                if (user.Password == password)
                {
                    return user;
                }
                throw new UnauthenticatedOperationException("Invalid credentials");
            }
            catch (EntityNotFoundException)
            {
                throw new UnauthorizedOperationException("Invalid username!");
            }
        }
    }
}
