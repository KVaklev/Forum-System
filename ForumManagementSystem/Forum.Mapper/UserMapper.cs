namespace ForumManagementSystem.Models
{
    public class UserMapper
    {
        public static GetUserDto MapUserToDtoGet(User user)
        {
            return new GetUserDto()
            {
                Email = user.Email,
                Username = user.Username,
                IsAdmin = user.IsAdmin,

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

            };
        }
    }

}