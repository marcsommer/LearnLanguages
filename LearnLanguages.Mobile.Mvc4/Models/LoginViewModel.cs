using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnLanguages.Mobile.Mvc4.Models
{
  public class LoginViewModel
  {
    [Required]
    [CustomValidation(typeof(Common.CommonHelper), "UsernameIsValidValidationResult")]
    public string Username { get; set; }

    [Required]
    [CustomValidation(typeof(Common.CommonHelper), "PasswordIsValidValidationResult")]
    public string Password { get; set; }
  }
}