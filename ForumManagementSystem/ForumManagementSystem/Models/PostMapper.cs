namespace ForumManagementSystem.Models
{
    public class PostMapper
    {
        public Post Map(PostDto postDto)
        {

            return new Post()
            {
                Title = postDto.Title,
                Content = postDto.Content,
            };
        }
    }
}
