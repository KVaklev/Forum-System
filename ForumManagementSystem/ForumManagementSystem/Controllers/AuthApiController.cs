﻿using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserMapper userMapper;
        private readonly AuthManager authManager;

        public AuthApiController(IUserService userService, UserMapper userMapper, AuthManager authManager)
        {
            this.userService = userService;
            this.userMapper = userMapper;
            this.authManager = authManager;
        }

        [HttpPost("login")] //needs implementation
        public IActionResult Login([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        //[HttpPost("logout")] //needs implementation
        //public IActionResult Logout([FromHeader] string credentials)
        //{
        //}

        //[HttpPost("register")] //needs implementation
        //public IActionResult Register([FromHeader] string credentials)
        //{
        //}
    }
}
