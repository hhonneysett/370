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
    
    public partial class Action
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Action()
        {
            this.Person_Session_Action_Log = new HashSet<Person_Session_Action_Log>();
            this.Role_Action = new HashSet<Role_Action>();
        }
    
        public int Action_ID { get; set; }
        public string Action_Name { get; set; }
        public string Action_Description { get; set; }
    
        public virtual Action Action1 { get; set; }
        public virtual Action Action2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person_Session_Action_Log> Person_Session_Action_Log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role_Action> Role_Action { get; set; }
    }
}
