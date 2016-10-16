using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryAssistantApp.Models
{
    [MetadataType(typeof(Possible_AnswerMetadata))]
    public partial class Possible_Answer
    {
    }

    public class Possible_AnswerMetadata
    {
        [Display(Name = "Question")]
        [Required]
        public int Question_Seq;

        [Display(Name = "Display Order")]
        [Required]
        public int Display_Order { get; set; }

        [Display(Name = "Answer")]
        [Required]
        public string Answer_Text { get; set; }


    }
}