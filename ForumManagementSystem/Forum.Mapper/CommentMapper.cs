namespace ForumManagementSystem.Models
{
    public class CommentMapper
    {

        public Comment Map(CommentDto dto)
        {
            return new Comment
            {
                UserId=dto.UserId,
                PostId=dto.PostId,
                Content = dto.Content
            };
        }

    }

}
