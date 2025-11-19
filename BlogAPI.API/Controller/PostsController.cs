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
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    //POST API CRUD Operations:
    // GET: api/posts => Retrieve all blog posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _postRepository.GetAllPostsAsync();
        return Ok(posts);
    }
    // GET: api/posts/{id} => Retrieve a specific post by ID
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
    // POST: api/posts => create a new blog post
    [HttpPost]
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
    // PUT: api/posts/{id} => update an entire post
    [HttpPut("{id}")]
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
    // PATCH: api/posts/{id} => partially update a post
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
    // DELETE: api/posts/{id} => delete a post and its comments
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        // Ensure the post exists before attempting deletion
        var deleted_post = await _postRepository.GetPostByIdAsync(id);
        if (deleted_post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        await _postRepository.DeletePostAsync(id);
        return NoContent();
    }
    //Endpoint: api/posts/{postId}/comments
    //get all comments for a post
    [HttpGet("{postId}/comments")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPostId(int postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return Ok(comments);
    }


    //Endpoint: api/posts/{postId}/comments
    //create a comment on a post
    [HttpPost("{postId}/comments")]
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
        await _commentRepository.CreateCommentAsync(comment);
        return Ok(comment);
    }

}