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
    
    public partial class Calender_Exception
    {
        public int Calender_Exception_Seq { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public System.TimeSpan Time_Open { get; set; }
        public System.TimeSpan Time_Closed { get; set; }
        public string Description { get; set; }
        public int Venue_ID { get; set; }
        public int Building_Floor_ID { get; set; }
        public int Building_ID { get; set; }
        public int Campus_ID { get; set; }
        public int HEX_Value { get; set; }
    
        public virtual Colour Colour { get; set; }
        public virtual Venue Venue { get; set; }
    }
}
