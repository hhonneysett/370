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
    
    public partial class Registered_Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registered_Person()
        {
            this.Person_Questionnaire = new HashSet<Person_Questionnaire>();
            this.Person_Role = new HashSet<Person_Role>();
            this.Person_Session_Log = new HashSet<Person_Session_Log>();
            this.Person_Topic = new HashSet<Person_Topic>();
            this.Questionnaires = new HashSet<Questionnaire>();
            this.Questionnaires1 = new HashSet<Questionnaire>();
            this.Venue_Problem = new HashSet<Venue_Problem>();
            this.Venue_Problem1 = new HashSet<Venue_Problem>();
            this.Trainer_Topic = new HashSet<Trainer_Topic>();
            this.Venue_Booking_Person = new HashSet<Venue_Booking_Person>();
        }
    
        public string Person_ID { get; set; }
        public string Person_Name { get; set; }
        public string Person_Surname { get; set; }
        public string Person_Email { get; set; }
        public string Person_Password { get; set; }
        public string Person_Registration_Status { get; set; }
        public System.DateTime Person_Registration_DateTime { get; set; }
        public int Person_Type_ID { get; set; }
        public string Person_Title { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person_Questionnaire> Person_Questionnaire { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person_Role> Person_Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person_Session_Log> Person_Session_Log { get; set; }
        public virtual Person_Title Person_Title1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person_Topic> Person_Topic { get; set; }
        public virtual Person_Type Person_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questionnaire> Questionnaires1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue_Problem> Venue_Problem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue_Problem> Venue_Problem1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trainer_Topic> Trainer_Topic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue_Booking_Person> Venue_Booking_Person { get; set; }
    }
}
