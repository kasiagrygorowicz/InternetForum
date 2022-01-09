using System;

namespace InternetForum.Infrastructure.Service
{
    public class ReplyDTO
    {
        public int Id { get; set; }
        public DateTime Posted { get; set; }
        public String Content { get; set; }
        public String AuthorUsername { get; set; }
    }
}