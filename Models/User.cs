using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace banetexam2.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;}
        public string Username {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public List<Idea> Ideas {get; set;}
        public List<Like> Likes {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}