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
    
    public partial class Document_Repository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document_Repository()
        {
            this.Document_Access_Log = new HashSet<Document_Access_Log>();
        }
    
        public int Document_Seq { get; set; }
        public string Document_Name { get; set; }
        public string Description { get; set; }
        public string Directory_Path { get; set; }
        public int Document_Extension_ID { get; set; }
        public int Category_ID { get; set; }
        public int Document_Type_ID { get; set; }
        public string Document_Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document_Access_Log> Document_Access_Log { get; set; }
        public virtual Document_Category Document_Category { get; set; }
        public virtual Document_Extension Document_Extension { get; set; }
        public virtual Document_Type Document_Type { get; set; }
    }
}
