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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _postRepository.GetAllPostsAsync();
        return Ok(posts);
    }
    // GET: api/posts/{id}
    [HttpGet("{id}")]
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
    // PUT: api/posts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Fetch existing post
        var existingPost = await _postRepository.GetPostByIdAsync(id);
        if (existingPost == null)
            return NotFound(new { message = $"Post with ID {id} not found" });

        // Update fields
        existingPost.Title = postDto.Title;
        existingPost.Content = postDto.Content;
        existingPost.UpdatedDate = DateTime.UtcNow;

        await _postRepository.UpdatePostAsync(existingPost);

        return Ok(existingPost);
    }
    // PATCH: api/tasks/{id}
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
    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        // Ensure the post exists before attempting deletion
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }

        await _postRepository.DeletePostAsync(id);
        return NoContent();
    }


}
