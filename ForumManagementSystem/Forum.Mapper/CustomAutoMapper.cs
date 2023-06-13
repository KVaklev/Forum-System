using AutoMapper;
using ForumManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumManagementSystem.Models
{
    public class CustomAutoMapper : Profile
    {
        public CustomAutoMapper()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<CommentDto, Comment>();
            CreateMap<PostDto, Post>();
            CreateMap<Post, PostDto>();
            CreateMap<GetUserDto, User>();
            CreateMap<CreateUserDto, User>(); 
            CreateMap<User, GetUserDto>();
            CreateMap<User, CreateUserDto>();
        }
    }
}
