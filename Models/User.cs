using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        public int UserId {get; set; }
        [Required]
        [MinLength(2, ErrorMessage="First name must be 2 characters or longer!")]
        public string FirstName {get; set; }
        [Required]
        [MinLength(2, ErrorMessage="Last name must be 2 characters or longer!")]
        public string LastName {get; set; }
        [EmailAddress]
        [Required]
        public string Email {get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        public Wedding OwnWedding {get; set;}
        public List<Guest> GoingTo {get; set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}