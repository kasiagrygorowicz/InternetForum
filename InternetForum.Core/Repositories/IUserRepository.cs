using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetForum.Core.Domain;

namespace InternetForum.Core.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task DelAsync(User user);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> BrowseAllAsync();
        Task<User> GetAsyncByEmail(string email);
        Task<User> GetAsyncById(String id);
        Task<User> GetAsyncByUserDetailsId(int id);
        
    }
}