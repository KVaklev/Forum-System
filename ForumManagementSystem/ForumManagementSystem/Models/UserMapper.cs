using ForumManagementSystem.Services;

namespace ForumManagementSystem.Models
{
    public class UserMapper
    {
        private readonly IUserService userService;

        public UserMapper(IUserService userService)
        {
            this.userService = userService;
        }
        public static GetUserDto MapUserToDtoGet(User user)
        {
            return new GetUserDto()
            {
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,

            };
        }

        public User MapUserToDtoCreate(CreateUserDto createUserDto)
        {
            return new User()
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Username = createUserDto.Username,
                Password = createUserDto.Password,
                Role = createUserDto.Role,

            };
        }

    }

}