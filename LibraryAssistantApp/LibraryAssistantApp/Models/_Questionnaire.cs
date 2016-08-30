using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryAssistantApp.Models
{
    [MetadataType(typeof(QuestionnaireMetadata))]
    public partial class Questionnaire
    {
    }

    public class QuestionnaireMetadata
    {
        [Display(Name = "Questionnaire name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Assessment Type")]
        [Required]
        public string Assessment_Type { get; set; }

        
        [Display(Name = "From")]
        [Required]
        public DateTime Active_From { get; set; }

        [Display(Name = "To")]
        [Required]
        public DateTime Active_To { get; set; }

        [Display(Name = "Create Date")]
        [Required]
        public DateTime Create_Date { get; set; }

        [Display(Name = "Topic")]
        [Required]
        public int Topic_Seq { get; set; }


    }
}