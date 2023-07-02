using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Repositories.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Models
{
    public class CommentViewModel
    {
                   
        [MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }
    }
}
