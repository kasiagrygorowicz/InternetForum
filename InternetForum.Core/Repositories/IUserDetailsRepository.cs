using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetForum.Core.Domain;

namespace InternetForum.Core.Repositories
{
    public interface IUserDetailsRepository
    {
        Task AddAsync(UserDetails userDetails);
        Task DelAsync(UserDetails userDetails);
        Task UpdateAsync(UserDetails userDetails);
        Task<IEnumerable<UserDetails>> BrowseAllAsync();
        Task<UserDetails> GetAsync(int id);
    }
}
