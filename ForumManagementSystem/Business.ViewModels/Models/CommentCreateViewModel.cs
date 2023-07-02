using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Models
{
	public class CommentCreateViewModel
	{
		[Required(ErrorMessage = "The {0} field is required")]
		[Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")] //TODO  - MaxValue - post.Count
		public int PostId { get; set; }

		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string Content { get; set; }
	}
}
