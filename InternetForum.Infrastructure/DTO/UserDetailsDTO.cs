using System;

namespace InternetForum.Infrastructure.Service
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Birthday { get; set; }
        public String UserId { get; set; }
    }
}