﻿using AutoMapper;
using Business.Exceptions;
using Business.Services.Contracts;
using DataAccess.Models;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;
using ForumManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace ForumManagementSystem.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private ICommentService commentService;
        private readonly IMapper mapper;
        private readonly AuthManager authManager;
        private readonly ILikeCommentService likeCommentService;

        public CommentApiController(
            ICommentService commentService, 
            IMapper mapper, 
            AuthManager authManager,
            ILikeCommentService likeCommentService)
        {
            this.commentService = commentService;
            this.mapper = mapper;
            this.authManager = authManager;
            this.likeCommentService = likeCommentService;
        }

        [HttpGet("")]
        public IActionResult GetComment([FromQuery] CommentQueryParameters filterParameters)
        {
            List<Comment> result = this.commentService.FilterBy(filterParameters);

            return this.StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            try
            {
                Comment comment = this.commentService.GetByID(id);

                return this.StatusCode(StatusCodes.Status200OK, comment);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }

        [HttpPost("")]
        public IActionResult CreateComment([FromBody] CommentDto commentDto, [FromHeader] string credentials)
        {
            try
            {
                Comment comment = this.mapper.Map<Comment>(commentDto);
                User user = this.authManager.TryGetUser(credentials);
                Comment createdComment = this.commentService.Create(comment, user);

                return this.StatusCode(StatusCodes.Status201Created, createdComment);
            }
            catch (DuplicateEntityException ex)
            {
                return this.StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CommentDto commentDto, [FromHeader] string credentials)
        {
            try
            {
                Comment comment = this.mapper.Map<Comment>(commentDto);
                User user = this.authManager.TryGetUser(credentials);
                Comment updatedComment = this.commentService.Update(id, comment, user);

                return this.StatusCode(StatusCodes.Status200OK, updatedComment);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);
                var deletedComment = this.commentService.Delete(id,user);

                return this.StatusCode(StatusCodes.Status200OK, deletedComment);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
        [HttpPut("{id}/like")]
        public IActionResult LikeDislikeComment(int id,[FromHeader] string credentials)
        {
            try
            {
                User userLoger = this.authManager.TryGetUser(credentials);
                Comment comment = this.commentService.GetByID(id);
                LikeComment likecomment = this.likeCommentService.Update(comment,userLoger);

                return this.StatusCode(StatusCodes.Status200OK);
            }
            catch (EntityNotFoundException ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }
    }
}
