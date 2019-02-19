using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        public int WeddingId { get;set; }
        [Required]
        public string WedderOne { get;set; }
        [Required]
        public string WedderTwo { get;set; }
        [Required]
        public DateTime Date { get;set; }
        [Required]
        public string Address { get;set; }
        public List<Guest> Guests {get; set;}
        public int UserId { get;set; }
        public User Owner { get;set; }
    }
}