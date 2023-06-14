﻿using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using System.Net;

namespace ForumManagementSystem.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly List<Comment> comments;
        private readonly ApplicationContext context;

        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public Comment Create(Comment comment, User user)
        {
            comment.UserId = user.Id;
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
        
        public List<Comment> FilterBy(CommentQueryParameters parameters)
        {
            List<Comment> result = context.Comments.ToList();

            if (parameters.UserId.HasValue)
            {
                result = result.FindAll(comment => comment.UserId==parameters.UserId);
            }
            if (parameters.FromDateTime.HasValue)
            {
                result = result.FindAll(c => c.DateTime >= parameters.FromDateTime);
            }
            if (parameters.ToDateTime.HasValue)
            {
                result = result.FindAll(c => c.DateTime <= parameters.ToDateTime);
            }
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("userId", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.UserId).ToList();
                }
                else if (parameters.SortBy.Equals("date", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.DateTime).ToList();
                }
                if (!string.IsNullOrEmpty(parameters.SortOrder)
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }
            return result;
        }

        public List<Comment> GetAll()
        {
            return context.Comments.ToList();
        }

        public Comment GetByID(int id)
        {
            var comment = context.Comments.FirstOrDefault(comment => comment.Id == id);
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
            commentToUpdate.DateTime = DateTime.Now;
            context.SaveChanges();

            return commentToUpdate;
        }
    }
}
