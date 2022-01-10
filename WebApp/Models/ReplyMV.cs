using System;
namespace WebApp.Models
{
    public class ReplyMV
    {
        public int Id { get; set; }
        public DateTime Posted { get; set; }
        public String Content { get; set; }
        public String AuthorUsername { get; set; }
        public int PostId { get; set; }
    }
}
