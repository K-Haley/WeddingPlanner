using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class LoginUser
    {
        [EmailAddress]
        [DisplayName("Email")]
        [Required]
        public string LEmail {get; set; }
        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Password")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string LPassword {get;set;}
        
    }
}