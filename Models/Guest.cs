using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Guest
    {
        public int GuestId { get;set; }
        public int UserId { get;set; }
        public int WeddingId { get;set; }
        public User Owner { get;set; }
        public Wedding Wedding { get;set; }

        public Guest(int userId, int weddingId)
        {
            this.UserId = userId;
            this.WeddingId = weddingId;
        }
    }
}