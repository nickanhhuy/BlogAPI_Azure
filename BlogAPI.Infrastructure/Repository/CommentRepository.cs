namespace BlogAPI.Infrastructure.Repository;
using BlogAPI.Core.Interfaces;
using BlogAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Infrastructure.Data;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }
    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return false;
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await _context.Comments
                         .Where(c => c.PostId == postId)
                         .ToListAsync();

    }

    public async Task<Comment?> UpdateCommentAsync(Comment comment)
    {
        var existingComment = await _context.Comments.FindAsync(comment.Id);
        if (existingComment == null)
        {
            return null;
        }
        existingComment.Name = comment.Name;
        existingComment.Email = comment.Email;
        existingComment.Content = comment.Content;
        existingComment.CreatedDate = comment.CreatedDate;
        await _context.SaveChangesAsync();
        return existingComment;
    }

    public async Task<bool> ExistsCommentAsync(int id)
    {
        return await _context.Comments.AnyAsync(c => c.Id == id);
    }
}
    

