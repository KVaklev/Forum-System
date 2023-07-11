using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumManagementSystem.Models
{
    public class User
    {
        // Collection navigation containing dependents
        public List<Post> Posts { get; set; } = new List<Post>();

        // Collection navigation containing dependents
        public List<Comment> Comments { get; set; } = new List<Comment> { };
        public int Id { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Please provide a valid email.")]
        public string Email { get; set; }

        [MinLength(5, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(30, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Username { get; set; }

        [Password]
        public string? Password { get; set; }

		public string? PhoneNumber { get; set; }

		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }

		public string? Country { get; set; }

		public bool IsBlocked { get; set; }

        public bool IsAdmin { get; set; }

        public string? ProfilePhotoPath { get; set; }

        public string? ProfilePhotoFileName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
        public List<LikeComment> LikeComments { get; set; } = new List<LikeComment>();
        public List<LikePost> LikePosts { get; set;} = new List<LikePost>();
      
    }
}
