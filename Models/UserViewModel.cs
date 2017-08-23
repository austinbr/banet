using System.ComponentModel.DataAnnotations;
using System;

namespace banetexam2.Models
{
    public class UserViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Required Field")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters!")]
        public string Name {get; set;}

        [Required(ErrorMessage = "Required Field")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters!")]
        public string Username {get; set;}

        [Required(ErrorMessage = "Required Field")]
        [EmailAddress(ErrorMessage = "Please enter a valid email!")]
        public string Email {get; set;}

        [Required(ErrorMessage = "Required Field")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [Required(ErrorMessage = "Required Field")]
        [Compare("Password", ErrorMessage="Passwords must match!")]
        [DataType(DataType.Password)]
        public string cPassword {get; set;}
    }
}