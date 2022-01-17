using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using System.Linq;

namespace InternetForum.Infrastructure.Service
{
    public class UserDetailsService : IUserDetailsService
    {
        private IUserRepository _userRepository;

        private IUserDetailsRepository _userDetailsRepository;

        public UserDetailsService(IUserRepository userRepository, IUserDetailsRepository userDetailsRepository)
        {

            _userRepository = userRepository;
            _userDetailsRepository = userDetailsRepository;
        }

        private UserDetails MapCreateUserDetailsToUserDetails(CreateUserDetails details)
        {
            return new UserDetails()
            {
                User_Id = details.Id,
                Birthday = details.Birthday,
                User = _userRepository.GetAsyncById(details.Id).Result

            };
        }


        public async Task AddAsync(CreateUserDetails details)
        {
            await _userDetailsRepository.AddAsync(MapCreateUserDetailsToUserDetails(details));
        }

        public async Task<IEnumerable<UserDetailsDTO>> BrowseAll()
        {
            var d = await _userDetailsRepository.BrowseAllAsync();
            return d.Select(details => MapUserDetailsToUserDetailsDTO(details));
        }

        private UserDetailsDTO MapUserDetailsToUserDetailsDTO(UserDetails details)
        {
            return new UserDetailsDTO()
            {

                Id = details.Id,
                Date = details.Date,
                Birthday = details.Birthday.ToString("MM/dd/yyyy"),
                UserId = details.User_Id
            };
        }

        public async Task DelAsync(int id)
        {
            var d = _userDetailsRepository.GetAsync(id).Result;
            await _userDetailsRepository.DelAsync(d);
        }

        public async Task<UserDetailsDTO> GetAsync(int id)
        {
            var d = await _userDetailsRepository.GetAsync(id);
            return MapUserDetailsToUserDetailsDTO(d);
        }

        public async Task UpdateAsync(EditUserDetails details, int id)
        {
            var d = MapEditUserDetailsToUserDetails(id, details);
          
            await _userDetailsRepository.UpdateAsync(d);
        }

        private  UserDetails MapEditUserDetailsToUserDetails(int id, EditUserDetails details)
        {
            User d = _userRepository.GetAsyncByUserDetailsId(id).Result;
            return new UserDetails()
            {
                Id = id,
                Birthday = details.Birthday,
                User = d,
                User_Id = d.Id,
                Date = d.UserDetails.Date




            };
        }
    }
}
