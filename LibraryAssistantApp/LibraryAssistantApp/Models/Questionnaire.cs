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
    
    public partial class Questionnaire
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Questionnaire()
        {
            this.Person_Questionnaire = new HashSet<Person_Questionnaire>();
            this.Questionnaire_Questions = new HashSet<Questionnaire_Questions>();
        }
    
        public int Questionnaire_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Assessment_Type { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Person_ID_Creator { get; set; }
        public System.DateTime Active_From { get; set; }
        public System.DateTime Active_To { get; set; }
        public Nullable<int> Topic_Seq { get; set; }
        public Nullable<int> Venue_Booking_Seq { get; set; }
        public Nullable<int> Venue_ID { get; set; }
        public Nullable<int> Building_Floor_ID { get; set; }
        public Nullable<int> Building_ID { get; set; }
        public Nullable<int> Campus_ID { get; set; }
        public string Person_ID_Involved { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person_Questionnaire> Person_Questionnaire { get; set; }
        public virtual Question_Topic Question_Topic { get; set; }
        public virtual Registered_Person Registered_Person { get; set; }
        public virtual Registered_Person Registered_Person1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questionnaire_Questions> Questionnaire_Questions { get; set; }
        public virtual Venue_Booking Venue_Booking { get; set; }
        public virtual Venue Venue { get; set; }
    }
}
