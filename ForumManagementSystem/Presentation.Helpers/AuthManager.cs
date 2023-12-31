﻿using Business.Exceptions;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Presentation.Helpers
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthManager(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
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

        public User TryGetUserByUsername(string username)
		{
            try
            {
                return this.userService.GetByUsername(username);
            }
            catch (EntityNotFoundException)
            {
                throw new UnauthorizedOperationException("Invalid username!");
            }
		}

		public User TryGetUser(string username, string password)
        {
            var user = TryGetUser(username+":"+password);

            //user = TryGetUserByUsername(username);

            //if (!user.CurrentPassword.Equals(password))
            //{
            //    throw new AuthenticationException("Invalid credentials!");
            //}

            return user;
        }

    }
}
