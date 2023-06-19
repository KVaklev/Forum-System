using DataAccess.Models;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Contracts
{
    public interface ILikePostService
    {
        LikePost Update(Post post, User user);

        
    }
}
