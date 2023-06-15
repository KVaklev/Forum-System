using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto
{
    internal class GetPostDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Username { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
