using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryAssistantApp.Models
{
    [MetadataType(typeof(Question_TopicMetadata))]
    public partial class Question_Topic
    {
    }

    public class Question_TopicMetadata
    {
        [Display(Name = "Topic")]
        public string Topic_Name;

        [Display(Name = "Topic")]
        public int Topic_Seq { get; set; }


    }
}