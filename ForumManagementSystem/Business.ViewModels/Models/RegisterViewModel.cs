using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Models
{
    public class RegisterViewModel
    {
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
        [MinLength(5, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(30, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Password]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? ProfilePhotoPath { get; set; }

        public string? ProfilePhotoFileName { get; set; }

    }
}
