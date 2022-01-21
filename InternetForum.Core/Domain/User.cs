using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace InternetForum.Core.Domain
{
    public class User : IdentityUser
    {
        public IEnumerable<Post> Posts  {get; set;}
        public IEnumerable<Reply> Replies  {get; set;}
        public UserDetails UserDetails { get; set; }

        public User()
        {
            Posts = new List<Post>();
            Replies = new List<Reply>();
        }

    }
}