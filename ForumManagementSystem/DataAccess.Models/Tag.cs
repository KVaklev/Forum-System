using ForumManagementSystem.Models;

namespace DataAccess.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Collection navigation containing dependents
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
