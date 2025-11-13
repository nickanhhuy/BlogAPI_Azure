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
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
    }
}