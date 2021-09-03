using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public class UserAccount
    {
        [Key]
        public int UserID { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
    
    public class RegistrationViewModel
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(120)]

        public string Email { get; set; }

        [Compare(nameof(Email))]
        [Display(Name ="Confirm Email")]
        public string ConfirmEmail { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 6, ErrorMessage ="Password must be between {2} and {1} characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name ="Confirm Password")]

        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)] //time is ignored
        public DateTime? DateOfBirth { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name ="Username or e-mail address")]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(120, MinimumLength = 6, ErrorMessage = "Password must be between {2} and {1} characters")]
        public string Password { get; set; }

    }
}
