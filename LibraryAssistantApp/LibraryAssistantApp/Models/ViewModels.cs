﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class RoleModel
    {
        [Required]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role name is required")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage ="Role name must be alphabetic and can only include one space")]
        public string RoleName { get; set; }
        public List<RoleActionModel> RoleActions { get; set; }
    }

    public class RoleActionModel
    {
        [Display(Name = "Create")]
        public bool CreateInd { get; set; }
        [Display(Name = "Read")]
        public bool ReadInd { get; set; }
        [Display(Name = "Update")]
        public bool UpdateInd { get; set; }
        [Display(Name = "Delete")]
        public bool DeleteInd { get; set; }
        public int ActionId { get; set; }
        public int RoleId { get; set; }
        [Display(Name = "Action")]
        public string ActionName { get; set; }

        public virtual RoleModel Role { get; set; }
        public virtual ActionModel Action { get; set; }
    }

    public class ActionModel
    {
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public List<RoleActionModel> ActionRoles { get; set; }
    }

    public class RoleIndexModel
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Action> Actions { get; set; }
        public IEnumerable<Role_Action> RoleActions { get; set; }
    }

    public class RoleEditModel
    {
        public Role role { get; set; }
        public List<Role_Action> actionList { get; set; }
    }
}