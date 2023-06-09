using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserMapper userMapper;
        private readonly AuthManager authManager;

        public UsersApiController(IUserService userService, UserMapper userMapper, AuthManager authManager)
        {
            this.userService = userService;
            this.userMapper=userMapper;
            this.authManager=authManager;
        }


        [HttpGet("")]
        public IActionResult GetUsers([FromQuery] UserQueryParameters userQueryParameters)
        {
            List<User> result = this.userService.FilterBy(userQueryParameters);

            List<GetUserDto> userDtos = result.Select(user => UserMapper.MapUserToDtoGet(user)).ToList();

            return this.StatusCode(StatusCodes.Status200OK, userDtos);
        }


        [HttpPost("")]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto) 
        {
            try
            {
                User user = this.userMapper.MapUserToDtoCreate(createUserDto);

                User createdUser = this.userService.Create(user);

                return this.StatusCode(StatusCodes.Status201Created, createdUser);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
          
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] CreateUserDto createUserDto)
        {
            try
            {
                User user = this.userMapper.MapUserToDtoCreate(createUserDto);

                User updatedUser = this.userService.Update(id, user);

                return this.StatusCode(StatusCodes.Status200OK, updatedUser);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
        }

        [HttpDelete("{id}")]// only by admin??
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var deletedBeer = this.userService.Delete(id);

                return this.StatusCode(StatusCodes.Status200OK, deletedBeer);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        [HttpPut("{id}/promote")]
        public IActionResult Promote(int id, [FromHeader] string credentials)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                if (loggedUser.IsAdmin)
                {
                    var user = this.userService.GetById(id);

                    var promotedUser = this.userService.Promote(user);

                    return StatusCode(StatusCodes.Status200OK, promotedUser);
                }
                return StatusCode(StatusCodes.Status405MethodNotAllowed);
            }
            catch (EntityNotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}