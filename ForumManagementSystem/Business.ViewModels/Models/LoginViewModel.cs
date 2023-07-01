using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
