using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InternetForum.Infrastructure.Repository
{
    public class ReplyRepository : IReplyRepository
    {
        private AppDbContext _appDbContext;

        public ReplyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Reply reply)
        {
            try
            {
                _appDbContext.Reply.Add(reply);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        public async Task<IEnumerable<Reply>> BrowseAllAsyncByPostId(int id)
        {
            return await Task.FromResult(_appDbContext.Reply
                .Include(reply => reply.Post)
                .Include(reply => reply.Author)
                .Where(reply => reply.Post.Id == id));
        }

        public async  Task<IEnumerable<Reply>> BrowseAllAsyncByUserId(String id)
        {
            return await Task.FromResult(_appDbContext.Reply
                .Where(reply => reply.Author.Id == id));
        }

        public async Task DelAsync(Reply reply)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Reply
                    .FirstOrDefault(r => r.Id == reply.Id));
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<Reply> GetAsync(int id)
        {
            return await Task.FromResult(
                _appDbContext.Reply
                .Include(reply => reply.Post)
                .Include(reply => reply.Author)
                .FirstOrDefault(reply => reply.Id == id));
        }

        public async Task UpdateAsync(Reply reply)
        {
            try
            {
                var oldReply = _appDbContext.Reply
                    .Include(reply => reply.Post)
                    .Include(reply => reply.Author)
                    .FirstOrDefault(r => r.Id == reply.Id);

                oldReply.Content = reply.Content;
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }
    }
}
