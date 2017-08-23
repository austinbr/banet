using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace banetexam2.Models
{
    public class Like : BaseEntity
    {
        [Key]
        public int Id {get; set;}
        public int Liker {get; set;}
        public int Liked {get; set;}
        public User LikerObj {get; set;}
        public Idea LikedObj {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}