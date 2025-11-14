using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Core.DTOs;
using BlogAPI.Core.Interfaces;
using BlogAPI.Core.Models;
namespace BlogAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IPostRepository _repository;
    private readonly ICommentRepository _commentRepository;
    public BlogController(IPostRepository repository, ICommentRepository commentRepository)
    {
        _repository = repository;
        _commentRepository = commentRepository;
    }
    //POST API CRUD Operations:
    // GET: api/posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _repository.GetAllAsync();
        return Ok(posts);
    }
    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Post with ID {id} not found" });
        }
        return Ok(post);
    }
    // POST: api/posts
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDto
    postDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var post = new Post
        {
            Title = postDto.Title,
            Description = postDto.Description,
            Status = postDto.Status,
            DueDate = postDto.DueDate
        };
        var createdPost = await _repository.CreateAsync(post);
        return CreatedAtAction(
            nameof(GetPost),
            new { id = createdPost.Id },
            createdPost
        );
    }

}
