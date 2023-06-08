namespace ForumManagementSystem.Models
{
    public class CategoryMapper
    {
        public Category Map(CategoryDTO categoryDTO)
        {
            return new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };
        }
    }
}
