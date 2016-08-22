﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LibraryAssistantEntities : DbContext
    {
        public LibraryAssistantEntities()
            : base("name=LibraryAssistantEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Attendee_Status> Attendee_Status { get; set; }
        public virtual DbSet<Attendee_Type> Attendee_Type { get; set; }
        public virtual DbSet<Booking_Status> Booking_Status { get; set; }
        public virtual DbSet<Booking_Type> Booking_Type { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Building_Floor> Building_Floor { get; set; }
        public virtual DbSet<Calender> Calenders { get; set; }
        public virtual DbSet<Calender_Exception> Calender_Exception { get; set; }
        public virtual DbSet<Calender_Rules> Calender_Rules { get; set; }
        public virtual DbSet<Campus> Campus { get; set; }
        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<Colour> Colours { get; set; }
        public virtual DbSet<Common_Problem> Common_Problem { get; set; }
        public virtual DbSet<Common_Problem_Type> Common_Problem_Type { get; set; }
        public virtual DbSet<Current_UP_Person> Current_UP_Person { get; set; }
        public virtual DbSet<Day_Of_Week> Day_Of_Week { get; set; }
        public virtual DbSet<Document_Access_Log> Document_Access_Log { get; set; }
        public virtual DbSet<Document_Category> Document_Category { get; set; }
        public virtual DbSet<Document_Extension> Document_Extension { get; set; }
        public virtual DbSet<Document_Repository> Document_Repository { get; set; }
        public virtual DbSet<Document_Type> Document_Type { get; set; }
        public virtual DbSet<Period_Type> Period_Type { get; set; }
        public virtual DbSet<Person_Level> Person_Level { get; set; }
        public virtual DbSet<Person_Questionnaire> Person_Questionnaire { get; set; }
        public virtual DbSet<Person_Questionnaire_Result> Person_Questionnaire_Result { get; set; }
        public virtual DbSet<Person_Role> Person_Role { get; set; }
        public virtual DbSet<Person_Session_Action_Log> Person_Session_Action_Log { get; set; }
        public virtual DbSet<Person_Session_Log> Person_Session_Log { get; set; }
        public virtual DbSet<Person_Title> Person_Title { get; set; }
        public virtual DbSet<Person_Type> Person_Type { get; set; }
        public virtual DbSet<Possible_Answer> Possible_Answer { get; set; }
        public virtual DbSet<Question_Bank> Question_Bank { get; set; }
        public virtual DbSet<Question_Topic> Question_Topic { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Questionnaire_Questions> Questionnaire_Questions { get; set; }
        public virtual DbSet<Style_Type> Style_Type { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<Venue_Booking> Venue_Booking { get; set; }
        public virtual DbSet<Venue_Booking_Person> Venue_Booking_Person { get; set; }
        public virtual DbSet<Venue_Problem> Venue_Problem { get; set; }
        public virtual DbSet<Venue_Type> Venue_Type { get; set; }
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Role_Action> Role_Action { get; set; }
        public virtual DbSet<Venue_Characteristic> Venue_Characteristic { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Topic_Category> Topic_Category { get; set; }
        public virtual DbSet<Trainer_Topic> Trainer_Topic { get; set; }
        public virtual DbSet<Person_Topic> Person_Topic { get; set; }
        public virtual DbSet<Registered_Person> Registered_Person { get; set; }
    
        public virtual ObjectResult<Venue> findBookingVenuesFunc(Nullable<System.DateTime> bookingStart, Nullable<System.DateTime> bookingEnd, string venueType, Nullable<int> campusID)
        {
            var bookingStartParameter = bookingStart.HasValue ?
                new ObjectParameter("bookingStart", bookingStart) :
                new ObjectParameter("bookingStart", typeof(System.DateTime));
    
            var bookingEndParameter = bookingEnd.HasValue ?
                new ObjectParameter("bookingEnd", bookingEnd) :
                new ObjectParameter("bookingEnd", typeof(System.DateTime));
    
            var venueTypeParameter = venueType != null ?
                new ObjectParameter("venueType", venueType) :
                new ObjectParameter("venueType", typeof(string));
    
            var campusIDParameter = campusID.HasValue ?
                new ObjectParameter("campusID", campusID) :
                new ObjectParameter("campusID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Venue>("findBookingVenuesFunc", bookingStartParameter, bookingEndParameter, venueTypeParameter, campusIDParameter);
        }
    
        public virtual ObjectResult<Venue> findBookingVenuesFunc(Nullable<System.DateTime> bookingStart, Nullable<System.DateTime> bookingEnd, string venueType, Nullable<int> campusID, MergeOption mergeOption)
        {
            var bookingStartParameter = bookingStart.HasValue ?
                new ObjectParameter("bookingStart", bookingStart) :
                new ObjectParameter("bookingStart", typeof(System.DateTime));
    
            var bookingEndParameter = bookingEnd.HasValue ?
                new ObjectParameter("bookingEnd", bookingEnd) :
                new ObjectParameter("bookingEnd", typeof(System.DateTime));
    
            var venueTypeParameter = venueType != null ?
                new ObjectParameter("venueType", venueType) :
                new ObjectParameter("venueType", typeof(string));
    
            var campusIDParameter = campusID.HasValue ?
                new ObjectParameter("campusID", campusID) :
                new ObjectParameter("campusID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Venue>("findBookingVenuesFunc", mergeOption, bookingStartParameter, bookingEndParameter, venueTypeParameter, campusIDParameter);
        }
    }
}
