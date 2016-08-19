using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Quartz;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Configuration;

namespace LibraryAssistantApp.Models
{
    public class EmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            LibraryAssistantEntities db = new LibraryAssistantEntities();

            var tomorrow = DateTime.Today.AddDays(1).Date;

            var upcommingBookings = (from b in db.Venue_Booking
                                     where b.Booking_Status.Equals("Active")
                                     select b).ToList();

            var temp = (from g in upcommingBookings
                        where g.DateTime_From.Date.Equals(tomorrow)
                        select g.Venue_Booking_Seq).ToList();

            var upcommingBookingsPerson = (from p in db.Venue_Booking_Person
                                           where temp.Contains(p.Venue_Booking_Seq)
                                           select p).ToList();

            foreach (var item in upcommingBookingsPerson)
            {
                var person = db.Registered_Person.Where(p => p.Person_ID.Equals(item.Person_ID)).FirstOrDefault();
                var booking = db.Venue_Booking.Where(v => v.Venue_Booking_Seq.Equals(item.Venue_Booking_Seq)).FirstOrDefault();
                var type = (from t in db.Booking_Type
                            where t.Booking_Type_Seq.Equals(booking.Booking_Type_Seq)
                            select t.Booking_Type_Name).FirstOrDefault();
                var timeslot = booking.DateTime_From.TimeOfDay + " " + booking.DateTime_To.TimeOfDay;
                var date = booking.DateTime_From.ToShortDateString();
                var campus = (from c in db.Campus
                              where c.Campus_ID.Equals(booking.Campus_ID)
                              select c.Campus_Name).FirstOrDefault();
                var building = (from b in db.Buildings
                                where b.Building_ID.Equals(booking.Building_ID)
                                select b.Building_Name).FirstOrDefault();
                var venue = (from v in db.Venues
                             where v.Venue_ID.Equals(booking.Venue_ID)
                             select v.Venue_Name).FirstOrDefault();

                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                message.From = new MailAddress("uplibraryassistant@gmail.com");
                message.To.Add(person.Person_Email);
                message.Subject = "Account Activation";
                message.Body = "Hi, " + person.Person_Name + ", don't forget about your booking taking place tomorrow: <hr/> <p>Type: " + type + "</p> <p>Timeslot: " + timeslot + "</p> <p>Date: " + date + "</p> <p>Campus: " + campus + "</p> <p>Building: " + building + "</p> <p>Venue: " + venue;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
                client.Send(message);
            }           
        }
    }
}