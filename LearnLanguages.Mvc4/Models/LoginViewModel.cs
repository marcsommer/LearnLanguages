﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnLanguages.Mvc4.Models
{
  public class LoginViewModel
  {
    [Required]
    [CustomValidation(typeof(Common.CommonHelper), "UsernameIsValidValidationResult")]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [CustomValidation(typeof(Common.CommonHelper), "PasswordIsValidValidationResult")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember Me?")]
    public bool RememberMe { get; set; }
  }
}