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
    
    public partial class Topic_Category
    {
        public int Topic_Category_ID { get; set; }
        public int Topic_Seq { get; set; }
        public int Category_ID { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
