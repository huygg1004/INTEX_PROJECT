using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(100, ErrorMessage ="The {0} must be at lest {2} characters long.", MinimumLength =6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm_password")]
        [Compare("Password", ErrorMessage ="The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
