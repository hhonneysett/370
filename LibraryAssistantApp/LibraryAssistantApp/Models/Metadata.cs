using System.ComponentModel.DataAnnotations;

namespace LibraryAssistantApp.Models
{
    public class RegisteredPersonMetadata
    {
        [Display(Name = "Student Number")]
        public string Person_ID;

        [Display(Name = "Name")]
        public string Person_Name;

        [Display(Name = "Surname")]
        public string Person_Surname;

        [Display(Name = "Email Address")]
        public string Person_Email;

        [Display(Name = "Password")]
        public string Person_Password;

        [Display(Name = "Level")]
        public int Level_ID;

        [Display(Name = "Title")]
        public string Person_Title;
    }

    public class PersonLevelMetadata
    {
        [Display(Name = "Level")]
        public string Level_Name;

        [Display(Name ="Level")]
        public int Level_ID;
    }

    public class PersonTitleMetadata
    {
        [Display(Name = "Title")]
        public string Person_Title1;
    }

    public class TopicMetadata
    {
        [Display(Name = "Topic")]
        public string Topic_Name;
    }

    public class DocumentRepositoryMetadata
    {
        [Display(Name ="File Name")]
        public string Document_Name;

        [Display(Name ="Directory Path")]
        public string Directory_Path;

        [Display(Name ="Category")]
        public string Category_ID;

        [Display(Name ="Extension Type")]
        public string Document_Extension_ID;

        [Display(Name ="Type")]
        public string Document_Type_ID;
    }

    public class DocumentCategoryMetadate
    {
        [Display(Name ="Category")]
        public string Category_Name;
    }

    public class DocumentExtensionMetadata
    {
        [Display(Name ="Extension Type")]
        public string Extension_Type;
    }

    public class DocumentTypeMetadata
    {
        [Display(Name ="Type")]
        public string Document_Type_Name;
    }

    public class CampusMetadata
    {
        [Display(Name ="Campus Name")]
        public string Campus_Name;
    }

    public class RoleMetadata
    {
        [Required(ErrorMessage ="Role name is required")]
        [Display(Name = "Role Name")]
        public string Role_Name;
    }
}

//    //public class PersonTitleMetadata
//    //{
//    //    [Display(Name ="Title")]
//    //    public string Person_Title1;
//    //}
//}