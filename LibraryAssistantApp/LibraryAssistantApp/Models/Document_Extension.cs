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
    
    public partial class Document_Extension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document_Extension()
        {
            this.Document_Repository = new HashSet<Document_Repository>();
        }
    
        public int Document_Extension_ID { get; set; }
        public string Extension_Type { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document_Repository> Document_Repository { get; set; }
    }
}
