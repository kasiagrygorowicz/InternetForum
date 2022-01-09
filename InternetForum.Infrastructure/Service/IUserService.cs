using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetForum.Infrastructure.Command;
using InternetForum.Infrastructure.DTO;

namespace InternetForum.Infrastructure.Service
{
    public interface IUserService
    {

        Task AddAsync(CreateUser user);
        Task DelAsync(String id);
        Task UpdateAsync(EditUser user,String id);
        Task<UserDTO> GetAsync(String id);
        Task<IEnumerable<UserDTO>> BrowseAll();

    }
}
