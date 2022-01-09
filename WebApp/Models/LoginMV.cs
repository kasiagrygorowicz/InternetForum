using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginMV
    {
       
            [Required(ErrorMessage = "Username required")]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Required(ErrorMessage = "Password required")]
            [Display(Name = "Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        
    }
}
