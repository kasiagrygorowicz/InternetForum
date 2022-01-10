using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RegisterMV
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }


    }
}
