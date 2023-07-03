using ForumManagementSystem.Models;

namespace Presentation.Helpers
{
    public interface IAuthManager
    {
        //User TryGetUser(string credentials);
        User TryGetUser(string username);
        User TryGetUser(string username, string password);

    }
}
