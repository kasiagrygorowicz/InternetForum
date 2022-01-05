using System.Collections.Generic;
using System.Threading.Tasks;
using InternetForum.Core.Domain;

namespace InternetForum.Core.Repositories
{
    public interface IPostRepository
    {

        Task AddAsync(Post post);
        Task DelAsync(Post post);
        Task UpdateAsync(Post post);
        Task<IEnumerable<Post>> BrowseAllAsync();
        Task<IEnumerable<Post>> BrowseAllAsyncByUser(int id);
        Task<Post> GetAsync(int id);
       
        
    }
}