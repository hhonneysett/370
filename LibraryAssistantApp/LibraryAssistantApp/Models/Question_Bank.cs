//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryAssistantApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question_Bank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question_Bank()
        {
            this.Possible_Answer = new HashSet<Possible_Answer>();
            this.Questionnaire_Questions = new HashSet<Questionnaire_Questions>();
        }
    
        public int Question_Seq { get; set; }
        public string Question_Text { get; set; }
        public int Topic_Seq { get; set; }
        public string Style_Type_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Possible_Answer> Possible_Answer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questionnaire_Questions> Questionnaire_Questions { get; set; }
        public virtual Question_Topic Question_Topic { get; set; }
        public virtual Style_Type Style_Type { get; set; }
    }
}
