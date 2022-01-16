using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using Microsoft.EntityFrameworkCore;
namespace InternetForum.Infrastructure.Repository
{
    public class PostRepository : IPostRepository
    {
        private AppDbContext _appDbContext;

        public PostRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Post post)
        {
            try
            {
                _appDbContext.Post.Add(post);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }


        public async Task<IEnumerable<Post>> BrowseAllAsync()
        {
            return await Task.FromResult(
                _appDbContext.Post
                .Include(post => post.Author)
                .Include(post => post.Replies));
        }

        public async Task<IEnumerable<Post>> BrowseAllAsyncByUser(String id)
        {
            return await Task.FromResult(
                _appDbContext.Post
                .Where(post => post.Author.Id == id));
        }

        public async Task DelAsync(Post post)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Post
                    .FirstOrDefault(p => p.Id == post.Id));
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task<Post> GetAsync(int id)
        {
            return await Task.FromResult(
                _appDbContext.Post.Include(post => post.Author)
                .Include(post => post.Replies)
                .FirstOrDefault(post => post.Id == id));
        }

        public async Task UpdateAsync(Post post)
        {
            try
            {
                var oldPost = _appDbContext.Post
                    .FirstOrDefault(p => p.Id == post.Id);
                Console.Write(post);
                oldPost.Description = post.Description;
                oldPost.Title = post.Title;
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }
    }
}

