using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.IO;

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
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be 5 characters long")]
        public string Person_Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Person_Password", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage ="Please confirm password")]
        public string Confirm_Password { get; set; }
    }

    public class UpdatePasswordModel
    {
        [Required(ErrorMessage = "Please provide current password"), Display(Name = "Current Password")]
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
        [Required, Display(Name = "Topic")]
        public int Topic_Seq { get; set; }
    }

    public class DeletePersonTopicModel
    {
        [Display(Name = "Topic")]
        public int Topic_Sequence { get; set; }
    }

    public class AddFileModel
    {
        [Required(ErrorMessage = "Please provide a file name"), MaxLength(30)]
        [Display(Name = "File Name")]
        [Remote("checkFilename", "Validate", ErrorMessage ="Filename already exists")]
        public string Document_Name { get; set; }

        [Required, MaxLength(50)]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int Category_ID { get; set; }

        [Display(Name = "Type")]
        public int Document_Type_ID { get; set; }

        [Display(Name = "Upload File"), Required(ErrorMessage = "Please provide a file")]
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
        [Required(ErrorMessage = "Please provide a file type name"), MaxLength(30)]
        [Display(Name = "File Type Name")]
        [Remote("checkFileType", "Validate", ErrorMessage ="File type already exists")]
        public string Type_Name { get; set; }

        [Display(Name = "File Type Description")]
        [Required, MaxLength(50)]
        public string Description { get; set; }
    }

    public class UpdateFileType
    {
        [Required(ErrorMessage = "Please provide a file type name"), MaxLength(30)]
        [Display(Name = "File Type Name")]
        public string Type_Name { get; set; }

        [Display(Name = "File Type Description")]
        [Required, MaxLength(50)]
        public string Description { get; set; }
    }

    public class AddFileCategory
    {
        [Required, MaxLength(30), Display(Name ="Category Name")]
        [Remote("checkFileCategory", "Validate", ErrorMessage ="Category already exits")]
        public string name { get; set; }

        [Required, MaxLength(50), Display(Name ="Description")]
        public string description { get; set; }
    }

    public class UpdateFileCategory
    {
        [Required, MaxLength(30), Display(Name = "Category Name")]
        public string name { get; set; }

        [Required, MaxLength(50), Display(Name = "Description")]
        public string description { get; set; }

        public int id { get; set; }
    }

    public class GetTypesModel
    {
        [Required]
        public int Category_ID { get; set; }
    }

    public class DiscussionRoomBooking
    {
        [Display(Name ="Person ID")]
        [Remote("loginCheckPerson", "Validate", ErrorMessage ="Invalid person ID")]
        public string person_id { get; set; }

        [Required(ErrorMessage ="Please provide a date selection")]
        [Display(Name ="Date")]
        public DateTime date { get; set; }

        public DateTime endDate { get; set; }

        [Required(ErrorMessage ="Please provide a time selection")]
        [Display(Name ="Time")]
        public string inTime { get; set; }
        public DateTime time { get; set; }

        [Required(ErrorMessage = "Please provide a length selection")]
        [Display(Name = "Duration")]
        public string length { get; set; }

        [Required(ErrorMessage = "Please provide a campus selection")]
        [Display(Name = "Campus")]
        public int campus_ID { get; set; }

        public string campus_name { get; set; }
    }

    public class EmpDiscussionRoomBooking
    {
        [Display(Name = "Username")]
        [Remote("loginCheckPerson", "Validate", ErrorMessage = "Not a registered person ID")]
        [Required(ErrorMessage ="Please enter a person ID")]
        public string person_id { get; set; }

        [Required(ErrorMessage = "Please provide a date selection")]
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        public DateTime endDate { get; set; }

        [Required(ErrorMessage = "Please provide a time selection")]
        [Display(Name = "Time")]
        public string inTime { get; set; }
        public DateTime time { get; set; }

        [Required(ErrorMessage = "Please provide a length selection")]
        [Display(Name = "Duration")]
        public string length { get; set; }

        [Required(ErrorMessage = "Please provide a campus selection")]
        [Display(Name = "Campus")]
        public int campus_ID { get; set; }

        public string campus_name { get; set; }
    }

    public class CampusModel
    {
        public int Campus_ID { get; set; }

        [Required(ErrorMessage = "Please provide a campus name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only alphabet characters allowed")]
        [Display(Name = "Campus Name")]
        public string Campus_Name { get; set; }
    }

    public class OneTimePinModel
    {
        [Required(ErrorMessage = "Please provide one time pin")]
        [Display(Name = "One Time Pin")]
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
        [Display(Name = "Duration")]
        public string length { get; set; }

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
        [Required(ErrorMessage ="Please provide a category name"), MaxLength(30)]
        [Remote("checkCategory", "Validate", ErrorMessage ="Category already exists")]
        public string categoryName { get; set; }

        [Display(Name ="Description")]
        [Required(ErrorMessage ="Please provide a description"), MaxLength(50)]
        public string description { get; set; }
  
    }

    public class TrainingSessionModel
    {
        [Required(ErrorMessage ="Please provide a category selection")]
        [Display(Name ="Category")]
        public int category { get; set; }

        [Required(ErrorMessage ="Please provide a topic selection")]
        [Display(Name ="Topic")]
        public int topic { get; set; }

        [Required(ErrorMessage ="Please provide a duration")]
        [Display(Name ="Duration (Minutes)")]
        public string duration { get; set; }

        [Required(ErrorMessage ="Please provide a start date")]
        [Display(Name ="Date")]
        public string startDate { get; set; }

        [Required(ErrorMessage = "Please provide a campus")]
        [Display(Name = "Campus")]
        public int campus { get; set; }
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
        public string building { get; set; }
        public string floor { get; set; }
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

        [Display(Name ="Description")]
        public string description { get; set; }

        [Display(Name ="Trainer")]
        public string trainer { get; set; }

        public string v_id { get; set; }

        [Display(Name ="Status")]
        public string status { get; set; }
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
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be 6 char long")]
        public string Person_Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Person_Password", ErrorMessage = "Confirm password does not match."), Display(Name = "Confirm Password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Please confirm password")]
        public string Confirm_Password { get; set; }
    }

    public class BookTrainingSessionModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string timeslot { get; set; }
        public string campus { get; set; }
        public string building { get; set; }            
        public string venue { get; set; }
    }

    public class PersonSessionReportModel
    {
        public Registered_Person person { get; set; }
        public int count { get; set; }
        public double totalTime { get; set; }
    }

    public class DocumentAccessReportModel
    {
        public Document_Repository document { get; set; }
        public int access { get; set; }
    }

    public class ActionAccessModel
    {
        public Action action { get; set; }
        public int access { get; set; }
    }

    public class VenueUsageModel
    {
        public Venue venue { get; set; }
        public int count { get; set; }
        public string campus { get; set; }
        public string building { get; set; }
    }

    public class TrainingSessionAttendance
    {
        public Venue_Booking training { get; set; }
        public int attended { get; set; }
        public int total { get; set; }
    }

    public class RoleModel
    {
        [Required]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role name is required")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Role name must be alphabetic and can only include one space")]
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

    public class PersonTypeModel
    {
        public IEnumerable<Person_Type> person_types { get; set; }
    }

    public class PersonTypeAddModel
    {
        [Required(ErrorMessage = "Person Type is required")]
        [Display(Name = "Person Type")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Person Type must be alphabetic and can only include one space")]
        public string person_type { get; set; }
    }

    public class PersonTypeEditModel
    {
        public Person_Type person_type { get; set; }
    }

    //Employee controllers
    public class EmployeeIndexModel
    {
        public IEnumerable<Registered_Person> registered_person { get; set; }
        public IEnumerable<Person_Role> person_role { get; set; }
        public List<Trainer_Topic> trainer_topic { get; set; }
        public IEnumerable<Role_Action> role_action { get; set; }
        public List<Role> roles { get; set; }
        //
        [Display(Name = "Username")]
        public string person_id { get; set; }
        [Display(Name = "Name")]
        public string person_name { get; set; }
        [Display(Name = "Surname")]
        public string person_surname { get; set; }
        [Display(Name = "Email")]
        public string person_email { get; set; }
    }

    public class EmployeeAddModel
    {
        public IEnumerable<Role_Action> role_action { get; set; }
        public List<Role> role { get; set; }
        public List<Topic_Category> topic_category { get; set; }
        public List<RoleCheck> role_check { get; set; }
        public List<TopicCheck> topic_check { get; set; }
        [Remote("UserExists", "Employee", ErrorMessage = "Username is already in use")]
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression("([p])([0-9]{8})+", ErrorMessage = "Username must begin with the letter 'p' and contain 8 numbers")]
        [Display(Name = "Username")]
        public string person_id { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Name cannot include numbers or special characters")]
        [Display(Name = "Name")]
        public string person_name { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Surname is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Surname cannot include numbers or special characters")]
        [Display(Name = "Surname")]
        public string person_surname { get; set; }
        [Remote("EmailExists", "Employee", ErrorMessage = "Email address is already in use")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address, please try again. Example: example@example.co.za")]
        [Display(Name = "Email Address")]
        public string person_email { get; set; }
        [Display(Name = "Person Type")]
        public string Person_Type { get; set; }
    }

    public class RoleCheck
    {
        public int role_id { get; set; }
        public bool role_ind { get; set; }
    }

    public class TopicCheck
    {
        public int topic_seq { get; set; }
        public string category_id { get; set; }
        public string topic_name { get; set; }
        public bool topic_ind { get; set; }
    }

    public class EmployeeEditModel
    {
        [Display(Name = "Username")]
        public string person_id {get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Name cannot include numbers or special characters")]
        [Display(Name = "Name")]
        public string person_name { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Surname is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Surname cannot include numbers or special characters")]
        [Display(Name = "Surname")]
        public string person_surname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address, please try again. Example: example@example.co.za")]
        [Display(Name = "Email Address")]
        public string person_email { get; set; }
        public List<EmpRoleCheckEdit> emprolecheckeditlist { get; set; }
        public List<TopicCheck> topicchecks { get; set; }
    }

    public class EmpRoleCheckEdit
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public bool role_ind { get; set; }
    }

    public class EmployeeDeleteModel
    {
        public Registered_Person registered_person { get; set; }
        public IEnumerable<Person_Role> person_role { get; set; }
        public List<Trainer_Topic> trainer_topic { get; set;  }
    }

    //member controllers
    public class MemberIndexVM
    {
        public IEnumerable<Registered_Person> registered_person { get; set; }
        [Display(Name = "Username")]
        public string person_id { get; set; }
        [Display(Name = "Name")]
        public string person_name { get; set; }
        [Display(Name = "Surname")]
        public string person_surname { get; set; }
        [Display(Name = "Email")]
        public string person_email { get; set; }
    }

    public class MemberCreateVM
    {
        [Remote("UserExists", "Member", ErrorMessage = "Username is already in use")]
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression("([u])([0-9]{8})+", ErrorMessage = "Username must begin with the letter 'u' and contain 8 numbers")]
        [Display(Name = "Username")]
        public string person_id { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Name cannot include numbers or special characters")]
        [Display(Name = "Name")]
        public string person_name { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Surname is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Surname cannot include numbers or special characters")]
        [Display(Name = "Surname")]
        public string person_surname { get; set; }
        [Remote("EmailExists", "Member", ErrorMessage = "Email address is already in use")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address, please try again. Example: example@example.co.za")]
        [Display(Name = "Email Address")]
        public string person_email { get; set; }
    }
    public class MemberEditVM
    {
        [Display(Name = "Username")]
        public string person_id { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Name cannot include numbers or special characters")]
        [Display(Name = "Name")]
        public string person_name { get; set; }
        [StringLength(30, ErrorMessage = "Maximum length is 30 characters")]
        [Required(ErrorMessage = "Surname is required")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Surname cannot include numbers or special characters")]
        [Display(Name = "Surname")]
        public string person_surname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address, please try again. Example: example@example.co.za")]
        [Display(Name = "Email Address")]
        public string person_email { get; set; }
    }

    public class MemberDeleteVM
    {
        public Registered_Person registered_person { get; set; }
        public IEnumerable<Person_Role> person_role { get; set; }
        public List<Trainer_Topic> trainer_topic { get; set; }
    }

    public class AuditLog
    {
        public string Action_Performed;
        public string Crud_Operation;
        public string Person_Name;
        public DateTime TimePerformed;
        public string Area;
        public int Session_ID { get; set; }
        public string Username { get; set; }
    }

    public class checkedCharacteristics
    {
        public Characteristic characteristic;
        public bool has;
    }

    public class monthList
    {
        public string month;
        public int count;
    }

    public class typeList
    {
        public string type;
        public int count;
    }

    public class problemList
    {
        public Venue_Problem problem;
        public string campus;
        public string building;
        public string floor;
    }

    public class problemTypeModel
    {
        [Display(Name ="Problem Type Name"), Required, MaxLength(30)]
        [Remote("checkProblemType", "Validate", ErrorMessage = "Problem type already exists")]
        public string name { get; set; }

        [Display(Name = "Problem Type Description"), Required, MaxLength(100)]
        public string description { get; set; }
    }

    public class commonProblemModel
    {
        public int Common_Problem_ID { get; set; }

        [Display(Name = "Problem Name"), Required, MaxLength(30)]
        public string Common_Problem_Name { get; set; }

        [Display(Name ="Description"), Required, MaxLength(100)]
        public string Description { get; set; }

        public int Common_Problem_Type_ID { get; set; }
    }

    public class UpdateCharModel
    {
        public int id { get; set; }

        [Display(Name ="Name"),Required, MaxLength(30)]
        public string name { get; set; }

        [Display(Name ="Description"), Required, MaxLength(100)]
        public string description { get; set; }
    }

    public class Clash
    {
        public string date { get; set; }
        public string topic { get; set; }
        public string timeslot { get; set; }
        public string trainer { get; set; }
        public string email { get; set; }
    }

    //document access report view model
    public class DocumentAccess
    {
        public List<Document_Access_Log> doc_access { get; set; }
        //public List<Document> documents { get; set; }
        public List<DocType> doc_types { get; set; }
        public List<Doc> docs { get; set; }
        public List<DocPerson> doc_persons { get; set; }
        public List<DocAcc> doc_acc { get; set; }
    }

    public class Document
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string person_id { get; set; }
        public string person_name { get; set; }
        public DateTime date_time { get; set; }
    }
    public class DocType
    {
        public string doc_type { get; set; }
        public int doc_count { get; set; }
    }
    public class Doc
    {
        public int doc_id { get; set; }
        public string doc_name { get; set; }
        public string doc_ext { get; set; }
        public int count { get; set; }
    }
    public class DocPerson
    {
        public string person_id { get; set; }
        public string person_name { get; set; }
        //public string person_surname { get; set; }
        public int access_count { get; set; }
    }
    public class DocAcc
    {
        public DateTime accessed { get; set; }
    }

    public class serverpath
    {
        public static string path = Path.Combine(HttpContext.Current.Server.MapPath("~"), "settings.xml");
    }

    public class BookingsReport
    {
        public List<BookingR> bookingsreportlist { get; set; }
        public BookingTotal Total { get; set; }
    }

    public class BookingR
    {
        public int total { get; set; }
        public string monthname { get; set; }
        public string monthyear { get; set; }
        public int trainingCount { get; set; }
        public int discussionCount { get; set; }
        public int confirmedCount { get; set; }
        public int activeCount { get; set; }
        public int cancelledCount { get; set; }
        public int tenativeCount { get; set; }
        public int completeCount { get; set; }
        public int studentCount { get; set; }
        public int trainerCount { get; set; }
        public int month { get; set; }
    }

    public class BookingTotal
    {
        public int discussionTotal { get; set; }
        public int trainingTotal { get; set; }
        public int studentTotal { get; set; }
        public int trainerTotal { get; set; }
        public int activeTotal { get; set; }
        public int confirmedTotal { get; set; }
        public int completeTotal { get; set; }
        public int cancelledTotal { get; set; }
        public int tenativeTotal { get; set; }
        public int bookingTotal { get; set; }
    }
}