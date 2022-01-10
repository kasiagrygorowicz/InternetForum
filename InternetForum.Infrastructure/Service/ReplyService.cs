using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;
using InternetForum.Infrastructure.Repository;

namespace InternetForum.Infrastructure.Service
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepository _replyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public ReplyService(IReplyRepository replyRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            _replyRepository = replyRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
           
        }

        public async Task AddAsync(CreateReply reply)
        {
            await _replyRepository.AddAsync(MapCreateReplyToReply(reply));
        }

        private Reply MapCreateReplyToReply(CreateReply reply)
        {
            return new Reply()
            {
                Content = reply.Content,
                Posted = DateTime.Now,
                Author = _userRepository.GetAsyncById(reply.Author).Result,
                Post = _postRepository.GetAsync(reply.Post).Result
            };
            }

        public async Task<IEnumerable<ReplyDTO>> BrowseAllByPostId(int id)
        {
            var r = await _replyRepository.BrowseAllAsyncByPostId(id);
            return r.Select(reply => MapReplyToReplyDTO(reply));
        }

        private ReplyDTO MapReplyToReplyDTO(Reply reply)
        {
            if (reply != null)
            {
                return new ReplyDTO()
                {
                    Id = reply.Id,
                    Posted = reply.Posted,
                    Content = reply.Content,
                    AuthorUsername = reply.Author.UserName,
                    PostId = reply.Post.Id

                };
            }
            else
            {
                return null;
            }
    

        }

        public async Task<IEnumerable<ReplyDTO>> BrowseAllByUserId(String id)
        {
            var r = await _replyRepository.BrowseAllAsyncByUserId(id);
            return r.Select(reply => MapReplyToReplyDTO(reply));
        }

        public  async Task DelAsync(int id)
        {
            Console.Write("/n/n");
            Console.Write(id);
            Console.Write("/n/n");
            var r =  _replyRepository.GetAsync(id).Result;
            await _replyRepository.DelAsync(r);
        }

        public async Task<ReplyDTO> GetAsync(int id)
        {
            return await Task.Run( () =>
            {
               return MapReplyToReplyDTO(_replyRepository.GetAsync(id).Result);
            });
        }

        public async Task UpdateAsync(EditReply reply, int id)
        {
            await _replyRepository.UpdateAsync(MapEditReplyToReply(reply, id));
        }

        private Reply MapEditReplyToReply(EditReply reply, int id)
        {
            var r = _replyRepository.GetAsync(id).Result;
            return new Reply()
            {
                Id = id,
                Content = reply.Content,
                Posted = r.Posted,
                Post = r.Post,
                Author = r.Author

            };
        }
    }
}
