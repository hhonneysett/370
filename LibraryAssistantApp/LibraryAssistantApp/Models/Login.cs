using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Student number required.", AllowEmptyStrings =false), Display(Name ="Person ID")]
        [Remote("loginCheckPerson", "Validate", ErrorMessage ="Not a registered person ID")]
        public string Person_ID { get; set; }

        [Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 8 char long")]
        public string Person_Password { get; set; }

        [Display(Name ="Remember Me?")]
        public bool RememberMe { get; set; }
    }
}