using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Models
{
    public class CategoryViewModel
    {
        [Required(AllowEmptyStrings = false, 
            ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MaxLength(25, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(5, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, 
            ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [MaxLength(1000, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(5, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string Description { get; set; }

      
    }
}
