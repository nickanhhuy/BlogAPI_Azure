using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Core.DTOs;
using BlogAPI.Core.Interfaces;
using BlogAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BlogAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    public CommentsController(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    [HttpGet]
    //get all comments
    public async  Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
    {
        var comments = await _commentRepository.GetAllCommentsAsync();
        return Ok(comments);
    }


    //get a specific comment by id
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        return Ok(comment);
    }

    //update an entire comment
    // PUT: api/comments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDto commentDto)
    {
        if (!ModelState.IsValid) {

            return BadRequest(ModelState);
        }

        var existingComment = await _commentRepository.GetCommentByIdAsync(id);
        if (existingComment == null) {
             return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        var comment = new Comment
        {
            Id = id,
            PostId = existingComment.PostId,
            Name = commentDto.Name,
            Email = commentDto.Email,
            Content = commentDto.Content
        };
        var updated_comment = await _commentRepository.UpdateCommentAsync(comment);
        return Ok(updated_comment);
    }

    //delete a comment
    //delete: api/comments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var deletedComment = await _commentRepository.GetCommentByIdAsync(id);
        if (deletedComment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        await _commentRepository.DeleteCommentAsync(id);
        return NoContent();
    }

    //update partially a comment
    //PATCH: api/comments/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchComment(int id, [FromBody] JsonPatchDocument<Comment> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest(new { message = "Patch document is null" });
        }
        var comment = await _commentRepository.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        patchDoc.ApplyTo(comment);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _commentRepository.UpdateCommentAsync(comment);
        return Ok(comment);
    }
}