using AspNetCoreDemo.Models;
using DataAccess.Models;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public interface IPostRepository
    {
        List<Post> GetAll();

        Post GetById(int id);

        Post GetByUser(User user);

        Post GetByTitle(string title);
        Tag GetByTag(string tag);

        Post GetByCategory(string category);

        PaginatedList<Post> FilterBy(PostQueryParameters parameters);

        Post Create(Post post, User user);

        Post Update(int id, Post post);

        Post Delete(int id);

        Category GetByCategoryId(int categoryId);

        User GetByUserId(int userId);

        void AddTagToPost(int tagId, int postId);

        bool TitleExists(string title);

    }
}
