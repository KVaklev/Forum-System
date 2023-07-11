using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Models
{
	public class UserUpdateProfileViewModel
	{
		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string? FirstName { get; set; }

		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string? LastName { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? DateOfBirth { get; set; }

		[EmailAddress(ErrorMessage = "Please provide a valid email.")]
		public string? Email { get; set; }

        [Password]
        public string? NewPassword { get; set; }

		public string? PhoneNumber { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? ProfilePhotoPath { get; set; }

        public string? ProfilePhotoFileName { get; set; }

        public string? Address { get; set; }

		public string? Country { get; set; }
		public bool? Admin { get; set; }
	}
}
