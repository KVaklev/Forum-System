using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumManagementSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserMapper userMapper;

        public UsersApiController(IUserService userService, UserMapper userMapper)
        {
            this.userService = userService;
            this.userMapper=userMapper;
        }

        [HttpGet("")]
        public IActionResult GetUsers()
        {
            List<User> users = this.userService.GetAll();

            List<GetUserDto> userDtos = users.Select(user => UserMapper.MapUserToDtoGet(user)).ToList();

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

        [HttpPut("{id}")] //only bi id??
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

        //[HttpPut("{username}")] //forbidden?


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
    }
}