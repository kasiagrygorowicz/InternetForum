using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.Core.Repositories;

namespace InternetForum.Infrastructure.Service
{
    public class PostService : IPostService
    {
        private  IPostRepository _postRepository;
        private  IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task AddAsync(CreatePost post)
        {
            await _postRepository.AddAsync(MapCreatePostToPost(post));
        }

        private Post MapCreatePostToPost(CreatePost post)
        {
            User author = _userRepository.GetAsyncById(post.Author).Result;
            return new Post()
            {

                Title = post.Title,
                Description = post.Description,
                Author = author


            };
        }

        public async Task<IEnumerable<PostDTO>> BrowseAll()
        {
            var p = await _postRepository.BrowseAllAsync();
            return p.Select(post => MapPostToPostDTO(post));
        }

        private PostDTO MapPostToPostDTO(Post post)
        {
            return new PostDTO()
            {
                Id = post.Id,
                Posted = post.Posted.ToString("MM/dd/yyyy"),
                Title = post.Title,
                Description = post.Description,
                AuthorNickname = post.Author.UserName
            };

        }

        public async Task DelAsync(int id)
        {
            await _postRepository.DelAsync(_postRepository.GetAsync(id).Result);
        }

        public async Task<PostDTO> GetAsync(int id)
        {

            return await Task.Run(() =>
            {
                return MapPostToPostDTO(_postRepository.GetAsync(id).Result);
            });
           
        }

        public async Task UpdateAsync(EditPost post, int id)
        {
            Post p = MapEditPostToPost(id, post);
            await _postRepository.UpdateAsync(MapEditPostToPost(id, post));
        }

        private Post MapEditPostToPost(int id, EditPost post)
        {
            Post  oldPost = _postRepository.GetAsync(id).Result;
            return new Post()
            {
                Id = id,
                Posted = oldPost.Posted,
                Title = post.Title,
                Author = oldPost.Author,
                Description = post.Description

            };
        }
    }
}
