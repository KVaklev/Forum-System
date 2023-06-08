using ForumManagementSystem.Services;

namespace Presentation.Helpers
{
    public class AuthManager
    {
        private readonly IUserService userService;

        public AuthManager(IUserService userService)
        {
            this.userService = userService;   
        }
    }
}
