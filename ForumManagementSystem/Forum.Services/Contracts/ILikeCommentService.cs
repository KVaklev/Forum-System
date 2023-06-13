using DataAccess.Models;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Contracts
{
    public interface ILikeCommentService
    {
        LikeComment Create(Comment comment, User user);

        LikeComment Update(Comment comment, User user);

        LikeComment Delete(Comment comment, User user);

        LikeComment Get(Comment comment, User user);
    }
}
