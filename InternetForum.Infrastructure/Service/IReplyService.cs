using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetForum.Infrastructure.Service
{
    public interface IReplyService
    {
        Task AddAsync(CreateReply reply);
        Task DelAsync(int id);
        Task UpdateAsync(EditReply reply, int id);
        Task<ReplyDTO> GetAsync(int id);
        Task<IEnumerable<ReplyDTO>> BrowseAllByPostId(int id);
        Task<IEnumerable<ReplyDTO>> BrowseAllByUserId(String id);
    }
}
