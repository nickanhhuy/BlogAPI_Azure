using BlogAPI.Core.Models;
namespace BlogAPI.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> AddCommentAsync(Comment comment);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}