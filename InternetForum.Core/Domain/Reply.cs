using System;

namespace InternetForum.Core.Domain
{
    public class Reply
    {
        public int Id { get; set; }
        public DateTime Posted { get; set; }
        public String Content { get; set; }
        public User Author { get; set; }
        public Post Post { get; set; }
    }
}