using System;
namespace InternetForum.Infrastructure.DTO
{
    public class UserDTO
    {
        public String Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; } 
        public DateTime Date { get; set; }
        public DateTime Birthday { get; set; }
        public int DetailsId { get; set; }

    }
}
