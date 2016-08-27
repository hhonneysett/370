using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Models
{
    public class UpdatePersonModel
    {
        [Required(ErrorMessage = "Please provide a name", AllowEmptyStrings = false), StringLength(30), Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Person_Name { get; set; }

        [Required(ErrorMessage = "Please provide a surname", AllowEmptyStrings = false), StringLength(30), Display(Name = "Surname")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Person_Surname { get; set; }

        [Required(ErrorMessage = "Please provide an email address"), StringLength(254), Display(Name = "Email Address"), RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
        ErrorMessage = "Please provide valid email id")]
        [Remote("checkUpdateEmail", "Validate", ErrorMessage = "Email is already in use")]
        public string Person_Email { get; set; }

        [Display(Name ="Title")]
        public string Person_Title { get; set; }
    }

    public class CreatePersonModel
    {
        [Display(Name ="Student Number")]
        [Required(ErrorMessage ="Please provide a student number")]
        [RegularExpression(@"[u][0-9]{8}", ErrorMessage ="Invalid student number")]
        [Remote("validateStudentNumber", "Validate", ErrorMessage ="You are not a student at TUKS or are already registered")]
        public string Person_ID { get; set; }

        [Required(ErrorMessage = "Please provide a name", AllowEmptyStrings = false), StringLength(30), Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Person_Name { get; set; }

        [Required(ErrorMessage = "Please provide a surname", AllowEmptyStrings = false), StringLength(30), Display(Name = "Surname")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Person_Surname { get; set; }

        [Required(ErrorMessage = "Please provide an email address"), StringLength(254), Display(Name = "Email Address"), RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
        ErrorMessage = "Please provide valid email address")]
        [Remote("checkEmail", "Validate", ErrorMessage ="Email is already in use")]
        public string Person_Email { get; set; }

        [Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 8 char long")]
        public string Person_Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Person_Password", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage ="Please confirm password")]
        public string Confirm_Password { get; set; }

        [Display(Name ="Title")]
        public string Person_Title { get; set; }
    }

    public class UpdatePasswordModel
    {
        [Required(ErrorMessage ="Please provide current password"), Display(Name ="Current Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Remote("currentPass", "Validate", ErrorMessage ="Password does not match your current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please provide new password", AllowEmptyStrings = false)]
        [Display(Name = "New Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 8 char long")]
        public string NewPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm New Password")]
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
        [Display(Name ="Person ID")]
        public string person_id { get; set; }

        [Required(ErrorMessage ="Please provide a date selection")]
        [Display(Name ="Date")]
        public DateTime date { get; set; }

        public DateTime endDate { get; set; }

        [Required(ErrorMessage ="Please provide a time selection")]
        [Display(Name ="Time")]
        public string inTime { get; set; }

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

    public class BookingDetailsModel
    {
        public int booking_seq { get; set; }

        [Display(Name ="Booking Status")]
        public string booking_status { get; set; }

        [Display(Name = "Person ID")]
        public string person_id { get; set; }

        [Display(Name ="Type")]
        public string type { get; set; }

        [Display(Name ="Date")]
        public string date { get; set; }

        [Display(Name ="Timeslot")]
        public string timeslot { get; set; }

        [Display(Name ="Campus")]
        public string campus { get; set; }

        [Display(Name ="Building")]
        public string building { get; set; }

        [Display(Name ="Venue")]
        public string venue { get; set; }
    }

    public class UpdateBookingModel
    {
        public int booking_seq { get; set; }

        [Display(Name = "Person ID")]
        public string person_id { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage ="Please provide a date")]
        public string date { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        [Required(ErrorMessage = "Please provide a time selection")]
        [Display(Name = "Time")]
        public string time { get; set; }

        [Required(ErrorMessage = "Please provide a length selection")]
        [Display(Name = "Duration (Minutes)")]
        public double length { get; set; }

        [Display(Name = "Campus")]
        [Required(ErrorMessage ="Please select a campus")]
        public string campus { get; set; }

        [Display(Name = "Building")]
        [Required(ErrorMessage = "Please select a building")]
        public string building { get; set; }

        [Display(Name = "Venue")]
        [Required(ErrorMessage = "Please select a venue")]
        public string venue { get; set; }

        public int campus_id { get; set; }

        public int venue_id { get; set; }

        public int building_id { get; set; }

        public int building_floor_id { get; set; }
    }

    public class CategoryModel
    {
        public int categoryId { get; set; }

        [Display(Name ="Category Name")]
        [Required(ErrorMessage ="Please provide a category name")]
        public string categoryName { get; set; }

        [Display(Name ="Description")]
        public string description { get; set; }
  
    }

    public class TrainingSessionModel
    {
        [Required(ErrorMessage ="Please provide a category selection")]
        [Display(Name ="Category")]
        public int Category_ID { get; set; }

        [Required(ErrorMessage ="Please provide a topic selection")]
        [Display(Name ="Topic")]
        public int Topic_ID { get; set; }

        [Required(ErrorMessage ="Please provide a duration")]
        [Display(Name ="Duration (Minutes)")]
        public int duration { get; set; }

        [Required(ErrorMessage ="Please provide a start date")]
        [Display(Name ="Date")]
        public string startDate { get; set; }

        [Required(ErrorMessage = "Please provide a campus")]
        [Display(Name = "Campus")]
        public int Campus_ID { get; set; }
    }

    public class timeslot
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

    public class venueTimeslot
    {
        public timeslot timeslot { get; set; }
        public IEnumerable<Venue> venues { get; set; }
    }

    public class venueRating
    {
        public Venue venue { get; set; }
        public double rating { get; set; }
        public string characteristics { get; set; }
    }

    public class TrainingDetailsModel
    {
        [Display(Name ="Person ID")]
        public string personId { get; set; }

        [Display(Name ="Booking Type")]
        public string bookingType { get; set; }

        [Display(Name ="Category")]
        public string category { get; set; }

        [Display(Name ="Topic")]
        public string topic { get; set; }

        [Display(Name ="Date")]
        public string date { get; set; }

        [Display(Name ="Timeslot")]
        public string timeslot { get; set; }

        [Display(Name ="Campus")]
        public string campus { get; set; }

        [Display(Name ="Building")]
        public string building { get; set; }

        [Display(Name ="Venue")]
        public string venue { get; set; }

        public bool attendance { get; set; }
    }

    public class AttendanceModel
    {
        public Registered_Person student { get; set; }
        public bool attended { get; set; }
    }

    public class StudentModel
    {
        [Display(Name ="Student Number")]
        [RegularExpression(@"[u][0-9]{8}", ErrorMessage = "Invalid student number")]
        public string personId { get; set; }
    }

    public class OneTimePin
    {
        [Display(Name ="One Time Pin")]
        [Remote("checkOTP", "Validate", ErrorMessage ="Invalid OTP")]
        public int OTP { get; set; }
    }

    public class LostPasswordModel
    {
        [Remote("checkRegPerson", "Validate", ErrorMessage ="Student number is not registered")]
        [RegularExpression(@"[u][0-9]{8}", ErrorMessage = "Invalid student number")]
        [Display(Name ="Student Number")]
        public string personId { get; set; }
    }

    public class ResetPassModel
    {
        [Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 6 char long")]
        public string Person_Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Person_Password", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Please confirm password")]
        public string Confirm_Password { get; set; }
    }

}