using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InternetForum.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task AddAsync(User user)
        {
            try
            {
                _appDbContext.User.Add(user);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task<IEnumerable<User>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.User
                .Include(user => user.UserDetails)
                .Include(user => user.Posts)
                .Include(user => user.Replies));
        }

        public async Task DelAsync(User user)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.User.FirstOrDefault(k => k.Id == user.Id));
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task<User> GetAsyncByEmail(string email)
        {
            return await Task.FromResult(_appDbContext.User
                .Include(user => user.UserDetails)
                .Include(user => user.Posts)
                .Include(user => user.Replies)
                .FirstOrDefault(user => user.Email == email));
        }

        public async Task<User> GetAsyncById(String id)
        {
            return await Task.FromResult(_appDbContext.User
               .Include(user => user.UserDetails)
               .Include(user => user.Posts)
               .Include(user => user.Replies)
               .FirstOrDefault(user => user.Id == id));
        }

        public async  Task<User> GetAsyncByUserDetailsId(int id)
        {
            return await Task.FromResult(_appDbContext.User
                .Include(user=>user.UserDetails)
                .Include(user => user.Posts)
                .Include(user => user.Replies)
                .FirstOrDefault(user => user.UserDetails.Id == id));
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                var oldUser = _appDbContext.User
               .Include(user => user.UserDetails)
               .Include(user => user.Posts)
               .Include(user => user.Replies)
               .FirstOrDefault(k => k.Id == user.Id);

                oldUser.UserName = user.UserName;
                _appDbContext.SaveChanges();

            }catch(Exception e)
            {
                await Task.FromException(e);
            }
        }
    }
}
