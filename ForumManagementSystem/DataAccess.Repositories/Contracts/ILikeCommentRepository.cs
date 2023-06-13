using DataAccess.Models;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Contracts
{
    public interface ILikeCommentRepository
    {
        LikeComment Get(Comment comment, User user);
        LikeComment Create(Comment comment, User user);
        LikeComment Update(Comment comment, User user);
        LikeComment Delete(Comment comment, User user);
    }
}
