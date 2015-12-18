using System;
using System.ComponentModel.DataAnnotations;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class UserCredits
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime DateTo { get; set; }//До когда выдано
        public string PrivateKey { get; set; } //Ключ, хранится в кукисах
        //Состоит из MD(email+UserAgent+random(5))
    }
}
