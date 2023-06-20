using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Dto
{
    public class GetPostDto
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int LikesCount { get; set; }


    }
}
