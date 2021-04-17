using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace demo_entity.Models
{
    public class Account
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Account")]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Password")]
        [StringLength(100,MinimumLength =6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    
        [Required]
        [Compare("Password",ErrorMessage ="The password and confirm password do not match")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
