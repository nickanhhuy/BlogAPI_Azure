using BlogAPI.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Core.Models;
using BlogAPI.Infrastructure.Data;
namespace BlogAPI.Infrastructure.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext _context;
        public PostRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.Include(p => p.Comments).ToListAsync();
        }
        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _context.Posts.
                Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Post> CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Post?> UpdatePostAsync(Post post)
        {
            var existingPost = await _context.Posts.FindAsync(post.Id);
            if (existingPost == null)
            {
                return null;
            }
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.Author = post.Author;
            existingPost.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingPost;
        }

        public async Task<bool> ExistsPostAsync(int id)
        {
            return await _context.Posts.AnyAsync(p => p.Id == id);
        }   
    }
}