﻿using AspNetCoreDemo.Models;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace ForumManagementSystem.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;

        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public Comment Create(Comment comment, User user)
        {
            comment.UserId = user.Id;
            comment.DateTime = DateTime.Now;
            comment.LikesCount = 0;
            context.Comments.Add(comment);
            context.SaveChanges();

            return comment;
        }

        public Comment Delete(int id)
        {
            Comment commentToDelete = this.GetByID(id);
            context.Comments.Remove(commentToDelete);
            context.SaveChanges();

            return commentToDelete;
        }
        
        public PaginatedList<Comment> FilterBy(CommentQueryParameters parameters)
        {
            var result = GetComments();

            if (parameters.UserId.HasValue)
            {
                result = result.FindAll(comment => comment.UserId==parameters.UserId);
            }
            if (!string.IsNullOrEmpty(parameters.Username))
            {
                result = result.Where(p => p.CreatedBy.Username == parameters.Username).ToList();
            }
            if (parameters.PostId.HasValue)
            {
                result = result.Where(p => p.PostId == parameters.PostId).ToList();
            }
            if (!string.IsNullOrEmpty(parameters.SortBy))
            { 
                if (parameters.SortBy.Equals("date", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.DateTime).ToList();
                }
                if (!string.IsNullOrEmpty(parameters.SortOrder)
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }
            if (result.Count==0)
            {
                throw new EntityNotFoundException("Тhere are no comments for the specified criteria.");
            }


            int totalPages = (result.Count() + parameters.PageSize - 1) / parameters.PageSize;

            result = Paginate(result, parameters.PageNumber, parameters.PageSize);

			return new PaginatedList<Comment>(result.ToList(), totalPages, parameters.PageNumber);
		}

		public static List<Comment> Paginate(List<Comment> result, int pageNumber, int pageSize)
		{
			return result
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
		}

		public List<Comment> GetAll()
        {
            return context.Comments.ToList();
        }

        public Comment GetByID(int id)
        {
            var comment = context.Comments
                .Include(u=>u.CreatedBy)
                .Include(p=>p.Post)
                .ThenInclude(c=>c.Category)
                .FirstOrDefault(comment => comment.Id == id);
                                
            return comment ?? throw new EntityNotFoundException($"Comment with ID = {id} doesn't exist.");
        }
        
        public Comment GetByUser(User user)
        {
            var comment = context.Comments.FirstOrDefault(comment => comment.UserId==user.Id);
            return comment ?? throw new EntityNotFoundException($"Comment with username {user.Username} doesn't exist.");
        }

        public Comment Update(int id, Comment comment)
        {
            Comment commentToUpdate = this.GetByID(id);
            commentToUpdate.Content = comment.Content;
            context.SaveChanges();

            return commentToUpdate;
        }

        private List<Comment> GetComments()
        { 
          List<Comment>result = context.Comments
                .Include(u => u.CreatedBy)
                .Include(p => p.Post)
                .ThenInclude(c => c.Category)
                .ToList();

          return result;
        }
    }
}
