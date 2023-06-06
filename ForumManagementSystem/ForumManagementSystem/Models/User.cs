﻿using System.ComponentModel.DataAnnotations;

namespace ForumManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(20, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(20, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Please provide a valid email.")]
        public string Email { get; set; }

        [MinLength(5, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(30, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public Role Role { get; set; }

        public bool IsBlocked = false;


        //public Post post { get; set; }


    }
}
