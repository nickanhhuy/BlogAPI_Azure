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
public class BlogController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    public BlogController(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    //POST API CRUD Operations:
    // GET: api/posts
    [HttpGet("posts")]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _postRepository.GetAllPostsAsync();
        return Ok(posts);
    }
    // GET: api/posts/{id}
    [HttpGet("posts/{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        return Ok(post);
    }
    // POST: api/posts
    [HttpPost("posts")]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDto postDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var post = new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            Author = "admin",
            CreatedDate = DateTime.UtcNow,

        };
        var createdPost = await _postRepository.CreatePostAsync(post);
        return CreatedAtAction(
            nameof(GetPost),
            new { id = createdPost.Id },
            createdPost
        );
    }
    // PUT: api/post/{id}
    [HttpPut("posts/{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Fetch existing post
        var existingPost = await _postRepository.ExistsPostAsync(id);
        if (!existingPost)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        var post = new Post
        {
            Id = id,
            Title = postDto.Title,
            Content = postDto.Content,
            Author = "admin",
            UpdatedDate = DateTime.UtcNow
        };
        var updatedPost = await _postRepository.UpdatePostAsync(post);

        return Ok(updatedPost);
    }
    // PATCH: api/posts/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTask(int id, [FromBody] JsonPatchDocument<Post> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest(new { message = "Patch document is null" });
        }
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        patchDoc.ApplyTo(post);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        post.UpdatedDate = DateTime.UtcNow;
        await _postRepository.UpdatePostAsync(post);
        return Ok(post);
    }
    // DELETE: api/posts/{id}
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        // Ensure the post exists before attempting deletion
        var deleted_post = await _postRepository.GetPostByIdAsync(id);
        if (deleted_post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        return NoContent();
    }

    // Comment CRUD operations API endpoints
    //get a specific comment by id
    [HttpGet("comments/{id}")]
    public async Task<ActionResult<Comment>> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        return Ok(comment);
    }
    //get all comments for a post
    [HttpGet("posts/{postId}/comments")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPostId(int postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return Ok(comments);
    }
    //create a comment on a post
    [HttpPost("posts/{postId}/comments")]
    public async Task<ActionResult<Comment>> CreateComment(int postId, [FromBody] CommentCreateDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = new Comment
        {
            PostId = postId,
            Name = commentDto.Name,
            Email = commentDto.Email,
            Content = commentDto.Content,
            CreatedDate = DateTime.UtcNow
        };
        var createdComment = await _commentRepository.CreateCommentAsync(comment);
        return CreatedAtAction(
            nameof(GetCommentById),
            new { id = createdComment.Id },
            createdComment
        );
    }
    //update an entire comment
    [HttpPut("comments/{id}")]
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
    [HttpDelete("comments/{id}")]
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
    [HttpPatch("comments/{id}")]
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