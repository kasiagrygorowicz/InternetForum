using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetForum.Core.Domain;

namespace InternetForum.Core.Repositories
{
    public interface ICommentRepository
    {
        Task AddAsync(Reply reply);
        Task UpdateAsync(Reply reply);
        Task DelAsync(Reply reply);
        Task<Reply> GetAsync(int id);
        //Task<IEnumerable<Reply>> BrowseAllAsync();
        //id belongs to post
        Task<IEnumerable<Reply>> BrowseAllAsyncByPostId(int id);
        Task<IEnumerable<Reply>> BrowseAllAsyncByUserId(int id);  
    }
}
