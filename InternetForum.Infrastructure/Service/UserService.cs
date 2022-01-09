using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using InternetForum.Infrastructure.Command;
using InternetForum.Infrastructure.DTO;

namespace InternetForum.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private User MapCreateUserToUser(CreateUser u)
        {
            return new User()
            {
                Email = u.Email,
                UserName = u.Username
            };
        }

        private UserDTO MapUserToUserDTO(User u)
        {
            return new UserDTO()
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                Date = u.UserDetails.Date,
                Birthday = u.UserDetails.Birthday,
                DetailsId = u.UserDetails.Id
            };
     
        }

        private User MapEditUserToUser(String id, EditUser u)
        {
            var oldUser = _userRepository.GetAsyncById(id).Result;
            return new User()
            {
                Id = id,
                UserDetails = oldUser.UserDetails,
                Posts = oldUser.Posts,
                Replies = oldUser.Replies,
                UserName = u.Username

        };
        }


        public async  Task AddAsync(CreateUser user)
        {
            User u = MapCreateUserToUser(user);
            await _userRepository.AddAsync(u);
        }

        public async Task<IEnumerable<UserDTO>> BrowseAll()
        {
           var u =  await _userRepository.BrowseAllAsync();
            return u.Select(user =>
            
                MapUserToUserDTO(user)
               
            );
               
        }

        public async Task DelAsync(String id)
        {
            var u = _userRepository.GetAsyncById(id).Result;
            await _userRepository.DelAsync(u);
        }

        public async Task<UserDTO> GetAsync(String id)
        {
            return await Task.FromResult(MapUserToUserDTO(_userRepository.GetAsyncById(id).Result));
        }

        public async Task UpdateAsync(EditUser user, String id)
        {
            await _userRepository.UpdateAsync(MapEditUserToUser(id, user));
        }
    }
}
