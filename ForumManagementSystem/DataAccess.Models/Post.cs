using DataAccess.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumManagementSystem.Models
{
    public class Post
    {

        public int Id { get; set; }

        // Navigation property
        [JsonIgnore]
        public User CreatedBy { get; set; }


        [Required(ErrorMessage = "The {0} field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")]  //TODO  - MaxValue - user.Count
        //Foreign key
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MinLength(16, ErrorMessage = "The {0} field must be at least {1} character.")]
        [MaxLength(64, ErrorMessage = "The {0} field must be less than {1} characters.")]
        public string Title { get; set; }


        // Navigation property
        [JsonIgnore]
        public Category Category { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")]
        // Foreign key
        public int CategoryId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MinLength(32, ErrorMessage = "The {0} field must be at least {1} character.")]
        [MaxLength(8192, ErrorMessage = "The {0} field must be less than {1} characters.")]
        public string Content { get; set; }

        // Collection navigation containing dependents
        public List<Comment> Comments { get; set; } = new List<Comment> { };

        public DateTime DateTime { get; set; }

        public int PostLikesCount { get; set; }

        // Collection navigation containing dependents
        public List<LikePost> LikePosts { get; set; } = new List<LikePost> { };
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
