using BlogAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BlogAPI.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post> CreatePostAsync(Post post);
        Task<Post?> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int id);
        Task<bool> ExistsPostAsync(int id);
    }
}