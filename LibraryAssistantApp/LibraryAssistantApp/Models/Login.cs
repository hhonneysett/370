﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Student number required.", AllowEmptyStrings = false), Display(Name = "Student Number")]
        public string Person_ID { get; set; }

        [Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters in length.")]
        public string Person_Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}