using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Models.ViewModel
{
    public class SignUpUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter username")]
        [Remote(action: "UserNameIsExist",controller:"Account")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [Display(Name ="Mobile Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public long? Mobile { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password",ErrorMessage =("confirm password can't matched!"))]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Active")]
        public bool IsActive { get; set; }
    }
}
