using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LibraryAssistantApp.Models
{
    public class MyIdentity : IIdentity
    {
        public IIdentity Identity { get; set; }
        public Registered_Person Registered_Person { get; set; }

        public MyIdentity(Registered_Person registered_person)
        {
            Identity = new GenericIdentity(registered_person.Person_ID);
            Registered_Person = registered_person;
        }

        public string AuthenticationType
        {
            get
            {
                return Identity.AuthenticationType;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return Identity.IsAuthenticated;
            }
        }

        public string Name
        {
            get
            {
                return Identity.Name;
            }
        }
    }
}