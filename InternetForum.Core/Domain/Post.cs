using System;
using System.Collections.Generic;

namespace InternetForum.Core.Domain
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime Posted { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public User Author { get; set; }
        public IEnumerable<Reply> Replies { get; set; }


        public Post()
        {
            Replies = new List<Reply>();
            Posted = DateTime.Now;
        }
    }
}