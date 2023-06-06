namespace ForumManagementSystem.Models
{
    public class CategoryMapper
    {
        public CategoryMapper()
        {

        }

        public Category Map(CategoryDTO dto)
        {
            return new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }
    }
}
