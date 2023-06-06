namespace ForumManagementSystem.Models
{
    public class UserMapper
    {
       public User Map(UserDto userDto)
        {
            return new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Username = userDto.Username,
                Role= userDto.Role,
                
            };
        }
    }
}
