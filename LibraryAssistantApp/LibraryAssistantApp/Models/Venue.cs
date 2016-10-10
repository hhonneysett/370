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
    
    public partial class Venue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venue()
        {
            this.Venue_Booking = new HashSet<Venue_Booking>();
            this.Venue_Characteristic = new HashSet<Venue_Characteristic>();
            this.Venue_Problem = new HashSet<Venue_Problem>();
        }
    
        public int Campus_ID { get; set; }
        public int Building_ID { get; set; }
        public int Building_Floor_ID { get; set; }
        public int Venue_ID { get; set; }
        public string Venue_Name { get; set; }
        public string Venue_Type { get; set; }
        public int Capacity { get; set; }
    
        public virtual Building Building { get; set; }
        public virtual Building_Floor Building_Floor { get; set; }
        public virtual Campus Campu { get; set; }
        public virtual Venue_Type Venue_Type1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue_Booking> Venue_Booking { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue_Characteristic> Venue_Characteristic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue_Problem> Venue_Problem { get; set; }
    }
}
