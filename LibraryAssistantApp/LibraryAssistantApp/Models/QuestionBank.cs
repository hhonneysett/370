using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryAssistantApp.Models
{
    [MetadataType(typeof(Question_BankMetadata))]
    public partial class Question_Bank
    {
    }

    public class Question_BankMetadata
    {
        [Display(Name = "Question")]        
        [Required]
        public string Question_Text { get; set; }

        
        [Display(Name = "Topic")]
        [Required]
        public int Topic_Seq { get; set; }

        
        [Display(Name = "Style Type")]
        [Required]
        public int Style_Type_ID { get; set; }
    }
}