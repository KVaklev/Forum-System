using ForumManagementSystem.Models;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public class PostTag
    {
        public int Id { get; set; }
        // Foreign key
        public int PostId { get; set; }
        public int TagId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Post? Post { get; set; }
        [JsonIgnore]
        public Tag? Tag { get; set; }

       
    }
}
