﻿using ForumManagementSystem.Models;

namespace ForumManagementSystem.Services
{
    public interface ICommentService
    {
        List<Comment> GetAll();

        Comment GetByID(int id);

        Comment GetByUser(User user);

        List<Comment> FilterBy(CommentQueryParameters parameters);

        Comment Create(Comment comment);

        Comment Update(int id, Comment comment);

        Comment Delete(int id);
    }
}