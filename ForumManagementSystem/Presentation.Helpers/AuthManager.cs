using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Presentation.Helpers
{
    public class AuthManager : IAuthManager
    {
        private const string CURRENT_USER = "CURRENT_USER";
        private readonly IUserService userService;
        private readonly IHttpContextAccessor contextAccessor;

        public AuthManager(IUserService userService, IHttpContextAccessor contextAccessor)
        {
            this.userService = userService;   
            this.contextAccessor = contextAccessor;
        }

        public void Login(string username, string password)
        {
            this.CurrentUser = this.TryGetUser(username, password);

            if (this.CurrentUser == null)
            {
                int? loginAttempts = this.contextAccessor.HttpContext.Session.GetInt32("LOGIN_ATTEMPTS");

                if (loginAttempts.HasValue && loginAttempts == 5)
                {
                    // redirect
                }
                else
                {
                    this.contextAccessor.HttpContext.Session.SetInt32("LOGIN_ATTEMPTS", (int)loginAttempts + 1);
                }
            }
        }

        public void Logout()
        {
            this.CurrentUser = null;
        }

        public User TryGetUser(string credentials)
        {
            string[] credentialsArray = credentials.Split(':');
            string username = credentialsArray[0];
            string password = credentialsArray[1];

            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            try
            {
                var user = this.userService.GetByUsername(username);
                if (user.Password == encodedPassword)
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

        public User TryGetUser(string username, string password)
        {
            try
            {
                User user = this.userService.GetByUsername(username);

                // check for password mismatch
                if (user.Password != password)
                {
                    throw new UnauthorizedOperationException("Invalid username or password");
                }

                return user;
            }
            catch (EntityNotFoundException)
            {
                throw new UnauthorizedOperationException("Invalid username or password");
            }
        }
        public User CurrentUser
        {
            get
            {
                try
                {
                    string username = this.contextAccessor.HttpContext.Session.GetString(CURRENT_USER);
                    User user = this.userService.GetByUsername(username);
                    return user;
                }
                catch (EntityNotFoundException)
                {
                    return null;
                }
            }
            set
            {
                // User
                User user = value;
                if (user != null)
                {
                    // add username to session
                    this.contextAccessor.HttpContext.Session.SetString(CURRENT_USER, user.Username);
                }
                else
                {
                    this.contextAccessor.HttpContext.Session.Remove(CURRENT_USER);
                }
            }
        }
    }
}
