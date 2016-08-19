using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryAssistantApp.Models
{
    public class UpdatePersonModel
    {
        [Required(ErrorMessage = "Please provide a name", AllowEmptyStrings = false), StringLength(30), Display(Name = "Name")]
        public string Person_Name { get; set; }

        [Required(ErrorMessage = "Please provide a surname", AllowEmptyStrings = false), StringLength(30), Display(Name = "Surname")]
        public string Person_Surname { get; set; }

        [Required(ErrorMessage = "Please provide an email address"), StringLength(254), Display(Name = "Email Address"), RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
        ErrorMessage = "Please provide valid email id")]
        public string Person_Email { get; set; }

        [Display(Name ="Level")]
        public int Level_ID { get; set; }

        [Display(Name ="Title")]
        public string Person_Title { get; set; }
    }

    public class CreatePersonModel
    {
        [Required(ErrorMessage = "Please provide a name", AllowEmptyStrings = false), StringLength(30), Display(Name = "Name")]
        public string Person_Name { get; set; }

        [Required(ErrorMessage = "Please provide a surname", AllowEmptyStrings = false), StringLength(30), Display(Name = "Surname")]
        public string Person_Surname { get; set; }

        [Required(ErrorMessage = "Please provide an email address"), StringLength(254), Display(Name = "Email Address"), RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
        ErrorMessage = "Please provide valid email id")]
        public string Person_Email { get; set; }

        [Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 8 char long")]
        public string Person_Password { get; set; }

        [Compare("Person_Password", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Confirm_Password { get; set; }

        public int Level_ID { get; set; }

        public string Person_Title { get; set; }
    }

    public class UpdatePasswordModel
    {
        [Required(ErrorMessage ="Please provide current password"), Display(Name ="Current Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please provide new password", AllowEmptyStrings = false)]
        [Display(Name = "New Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 8 char long")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm New Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }

    public class AddPersonTopicModel
    {
        [Required, Display(Name ="Topic")]
        public int Topic_Seq { get; set; }
    }

    public class DeletePersonTopicModel
    {
        [Display(Name ="Topic")]
        public int Topic_Sequence { get; set; }
    }

    public class AddFileModel
    {
        [Required(ErrorMessage ="Please provide a file name")]
        [Display(Name ="File Name")]
        public string Document_Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Category")]
        public int Category_ID { get; set; }

        [Display(Name ="Type")]
        public int Document_Type_ID { get; set; }

        [Display(Name ="Upload File"), Required(ErrorMessage ="Please provide a file")]
        public HttpPostedFileBase uploadFile { get; set; }
    }

    public class UpdateFileModel
    {
        public int Document_Seq { get; set; }

        [Required(ErrorMessage = "Please provide a file name")]
        [Display(Name = "File Name")]
        public string Document_Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Category")]
        public int Category_ID { get; set; }

        [Display(Name = "Type")]
        public int Document_Type_ID { get; set; }

        [Display(Name = "Upload File")]
        public HttpPostedFileBase uploadFile { get; set; }
    }

    public class DeleteFileModel
    {
        public int Document_Seq { get; set; }

        [Display(Name = "File Name")]
        public string Document_Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Type")]
        public string Document_Type_Name { get; set; }
    }

    public class AddFileTypeModel
    {
        [Required(ErrorMessage ="Please provide a file type name")]
        [Display(Name ="File Type Name")]
        public string Type_Name { get; set; }

        [Display(Name ="File Type Description")]
        public string Description { get; set; }
    }

    public class GetTypesModel
    {
        [Required]
        public int Category_ID { get; set; }
    }

    public class DiscussionRoomBooking
    {
        [Required(ErrorMessage ="Please provide a date selection")]
        [Display(Name ="Date")]
        public DateTime date { get; set; }

        public DateTime endDate { get; set; }

        [Required(ErrorMessage ="Please provide a time selection")]
        [Display(Name ="Time")]
        public DateTime time { get; set; }

        [Required(ErrorMessage ="Please provide a length selection")]
        [Display(Name ="Duration (Minutes)")]
        public int length { get; set; }

        [Required(ErrorMessage ="Please provide a campus selection")]
        [Display(Name ="Campus")]
        public int campus_ID { get; set; }

        public string campus_name { get; set; }
    }

    public class CampusModel
    {
        public int Campus_ID { get; set; }

        [Required(ErrorMessage ="Please provide a campus name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage ="Only alphabet characters allowed")]
        [Display(Name ="Campus Name")]
        public string Campus_Name { get; set; }
    }

    public class OneTimePinModel
    {
        [Required(ErrorMessage ="Please provide one time pin")]
        [Display(Name ="One Time Pin")]
        public string pin { get; set; }
    }
}