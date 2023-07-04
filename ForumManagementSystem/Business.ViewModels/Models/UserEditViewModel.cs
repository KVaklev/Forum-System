using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Models
{
	public class UserEditViewModel
	{
		public int Id { get; set; }
		[Required]
		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string FirstName { get; set; }

		[Required]
		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string LastName { get; set; }

		[Required]
		[EmailAddress(ErrorMessage = "Please provide a valid email.")]
		public string Email { get; set; }

		[Required]
		[Password]
		public string Password { get; set; }

		public string? PhoneNumber { get; set; }
	}
}
