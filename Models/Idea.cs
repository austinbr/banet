using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace banetexam2.Models
{
    public class Idea : BaseEntity
    {
        [Key]
        public int Id {get; set;}
        public string Content {get; set;}
        public int CreatedBy {get; set;}
        public User CreatedByObj {get; set;}
        public List<Like> Likes {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}