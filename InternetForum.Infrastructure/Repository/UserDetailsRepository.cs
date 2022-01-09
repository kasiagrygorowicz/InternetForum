using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InternetForum.Infrastructure.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private AppDbContext _appDbContext;

        public UserDetailsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(UserDetails userDetails)
        {
            try
            {
                _appDbContext.UserDetails.Add(userDetails);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<IEnumerable<UserDetails>> BrowseAllAsync()
        {
            return await Task.FromResult(
                _appDbContext.UserDetails
                .Include(user => user.User)
                .Include(user => user.Id));
        }

        public async Task DelAsync(UserDetails userDetails)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.UserDetails
                    .FirstOrDefault(ud=> ud.Id == userDetails.Id));
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }

        }

            public async Task<UserDetails> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.UserDetails.Include(u => u.User).FirstOrDefault(u => u.Id == id));
        }

        public async Task UpdateAsync(UserDetails userDetails)
        {
            try
            {
                var oldUserDetails = _appDbContext.UserDetails
                    .Include(ud => ud.User)
                    .FirstOrDefault(u => u.Id == userDetails.Id);

            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }
    }
}
