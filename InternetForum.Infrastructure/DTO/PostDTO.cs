using System;

namespace InternetForum.Infrastructure.Service
{
    public class PostDTO
    {
        public int Id { get; set; }
        public String Posted { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String AuthorNickname { get; set; }
    }
}