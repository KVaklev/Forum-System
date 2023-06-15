using DataAccess.Models;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Contracts
{
    public interface ILikePostRepository
    {
        LikePost Get(Post post, User user);

        LikePost Create(Post post, User user);

        LikePost Update(Post post, User user);

        LikePost Delete(Post post, User user);
    }
}
