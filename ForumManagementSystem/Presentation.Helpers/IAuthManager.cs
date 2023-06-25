using ForumManagementSystem.Models;

namespace Presentation.Helpers
{
    public interface IAuthManager
    {
        User TryGetUser(string credentials);
    }
}
