using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Dto
{
    internal class GetPostDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Username { get; set; }

        public DateTime DateCreated { get; set; }

        //        CreatedBy: Username
        //Datetime:
        //title
        //Category: Name
        //Content
        //Likes:Count
        
    }
}
