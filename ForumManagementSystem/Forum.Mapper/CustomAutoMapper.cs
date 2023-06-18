using AutoMapper;
using Business.Dto;
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
            CreateMap<CreatePostDto, Post>();
            CreateMap<Post, CreatePostDto>();
            CreateMap<GetUserDto, User>();
            CreateMap<CreateUserDto, User>(); 
            CreateMap<User, GetUserDto>();
            CreateMap<User, CreateUserDto>();
            CreateMap<User, UpdateUserDto>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
