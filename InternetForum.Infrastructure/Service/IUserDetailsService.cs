using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetForum.Infrastructure.Service
{
    public interface IUserDetailsService
    {
        Task AddAsync(CreateUserDetails details);
        Task DelAsync(int id);
        Task UpdateAsync(EditUserDetails details,int id);
        Task<UserDetailsDTO> GetAsync(int id);
        Task<IEnumerable<UserDetailsDTO>> BrowseAll();
    }
}
