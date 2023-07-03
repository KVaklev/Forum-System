﻿using AutoMapper;
using Business.Dto;
using Business.ViewModels.Models;
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
            CreateMap<Comment, RequireCommentDto>()
                .ForMember(c => c.Username, u => u.MapFrom(c => c.CreatedBy.Username))
                .ForMember(c => c.categoryName, u => u.MapFrom(c => c.Post.Category.Name));
            CreateMap<CreatePostDto, Post>();
            CreateMap<Post, CreatePostDto>();
            CreateMap<GetPostDto, Post>();
            CreateMap<Post, GetPostDto>()
            .ForMember(c => c.Username, u => u.MapFrom(c => c.CreatedBy.Username))
                .ForMember(c => c.CategoryName, u => u.MapFrom(c => c.Category.Name))
                .ForMember(c => c.DateCreated, u => u.MapFrom(c => c.DateTime));
            CreateMap<GetUserDto, User>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, GetUserDto>();
            CreateMap<User, CreateUserDto>();
            CreateMap<User, UpdateUserDto>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, LoginViewModel>();
            CreateMap<LoginViewModel, User>();
            CreateMap<User, RegisterViewModel>();
            CreateMap<RegisterViewModel,User>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
            CreateMap<Post, PostViewModel>();
            // .ForMember(c => c.CategoryId, u => u.MapFrom(c => c.Category.Name));
            CreateMap<PostViewModel, Post>();
            CreateMap<Comment, CommentGetViewModel>()
                .ForMember(c => c.Username, u => u.MapFrom(c => c.CreatedBy.Username))
                .ForMember(c => c.PostTitle, u => u.MapFrom(c => c.Post.Title))
                .ForMember(c => c.CategoryName, u => u.MapFrom(c => c.Post.Category.Name));
            CreateMap<Comment, CommentViewModel>();
            CreateMap<Comment, CommentCreateViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<CommentCreateViewModel, Comment>();
            CreateMap<CommentReplyCreateViewModel, Comment>();

        }
    }
}
