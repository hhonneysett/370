using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.IO;

namespace LibraryAssistantApp.Models
{
    public class RegisteredPersonMetadata
    {

        [Display(Name = "Username")]

        public string Person_ID;

        [Display(Name = "Name")]
        public string Person_Name;

        [Display(Name = "Surname")]
        public string Person_Surname;

        [Display(Name = "Email Address")]
        public string Person_Email;

        [Display(Name = "Password")]
        public string Person_Password;

        [Display(Name = "Type")]
        public string Person_Type;
    }

    public class PersonLevelMetadata
    {
        [Display(Name = "Level")]
        public string Level_Name;

        [Display(Name = "Level")]
        public int Level_ID;
    }

    public class TopicMetadata
    {
        [Display(Name = "Topic")]
        [Required(ErrorMessage = "Please provide a topic name")]
        public string Topic_Name;
    }

    public class DocumentRepositoryMetadata
    {
        [Display(Name = "File Name")]
        public string Document_Name;

        [Display(Name = "Directory Path")]
        public string Directory_Path;

        [Display(Name = "Category")]
        public string Category_ID;

        [Display(Name = "Extension Type")]
        public string Document_Extension_ID;

        [Display(Name = "Type")]
        public string Document_Type_ID;
    }

    public class DocumentCategoryMetadate
    {
        [Display(Name = "Category")]
        public string Category_Name;
    }

    public class DocumentExtensionMetadata
    {
        [Display(Name = "Extension Type")]
        public string Extension_Type;
    }

    public class DocumentTypeMetadata
    {
        [Display(Name = "Type")]
        public string Document_Type_Name;
    }

    public class CampusMetadata
    {
        [Display(Name = "Campus Name")]
        public string Campus_Name;
    }

    public class CategoryMetadata
    {
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Please provide a category name")]
        public string Category_Name;
    }

    public class RoleMetadata
    {
        [Required(ErrorMessage = "Role name is required")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Role name must be alphabetic and can only include one space")]
        [Display(Name = "Role")]
        public string Role_Name;
    }

    public class PersonTypeMetadata
    {
        [Display(Name = "Type")]
        [Required(ErrorMessage = "Person Type required")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Person Type must be alphabetic and can only include one space")]
        public string Person_Type1 { get; set; }

    }

    public class ProblemTypeMetadata
    {
        [Display(Name = "Problem Type")]
        public int Common_Problem_Type_ID { get; set; }

        [Display(Name = "Common Problem Name"), Required, MaxLength(30)]
        public string Common_Problem_Type_Name { get; set; }

        [Display(Name = "Description"), Required, MaxLength(100)]
        public string Description { get; set; }
    }

    public class CommonProblemMetadata
    {
        [Display(Name = "Problem Name"), Required, MaxLength(30)]
        [Remote("checkProblem", "Validate", ErrorMessage = "Problem already exists")]
        public string Common_Problem_Name { get; set; }

        [Display(Name = "Problem Type"), Required]
        public int Common_Problem_Type_ID { get; set; }

        [Display(Name = "Description"), Required, MaxLength(100)]
        public string Description { get; set; }

    }

    public class CharacteristicModel
    {
        [Display (Name ="Name"), MaxLength(30), Required]
        [Remote("checkCharacteristic", "Validate", ErrorMessage = "Characteristic already exists")]
        public string Characteristic_Name { get; set; }

        [Display(Name ="Description"), Required, MaxLength(100)]
        public string Description { get; set; }
    }
}
