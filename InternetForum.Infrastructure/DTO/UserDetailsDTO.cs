using System;

namespace InternetForum.Infrastructure.Service
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String Birthday { get; set; }
        public String UserId { get; set; }
    }
}