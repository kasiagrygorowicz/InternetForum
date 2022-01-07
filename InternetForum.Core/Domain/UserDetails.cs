using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace InternetForum.Core.Domain
{
    public class UserDetails
    {
       
        public int Id { get; set; }
        public User User { get; set; }
        public int User_Id { get; set; }
        // Date of creating the account 
        public DateTime Date { get; set; }
        public DateTime Birthday { get; set; }


        public UserDetails()
        {
            Date = DateTime.Now;
        }
    }

    
}