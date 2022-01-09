using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetForum.Infrastructure.Service
{
    public interface IPostService
    {
        
        Task AddAsync(CreatePost post);
        Task DelAsync(int id);
        Task UpdateAsync(EditPost post,int id);
        Task<PostDTO> GetAsync(int id);
        Task<IEnumerable<PostDTO>> BrowseAll();
        
    }
}
