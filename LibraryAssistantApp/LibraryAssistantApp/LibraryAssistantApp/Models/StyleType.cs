using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryAssistantApp.Models
{
    [MetadataType(typeof(Style_TypeMetadata))]
    public partial class Style_Type
    {
    }

    public class Style_TypeMetadata
    {
        [Display(Name = "Style Type")]
        [Required]
        [Key]
        public string Style_Type_ID { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}