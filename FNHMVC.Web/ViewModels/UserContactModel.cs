using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNHMVC.Web.ViewModels
{
    public class UserContactModel
    {

        public string Subject { get; set; }
        public string Message { get; set; }
        public long UserIdToContact { get; set; }
        public bool SentEmail { get; set; }
        public string LastMessage { get; set; }
        public UserContactModel(long UserIdToContact)
        {
            this.UserIdToContact = UserIdToContact;
        }

        public UserContactModel(long UserIdToContact, string LastMessage)
        {
            this.UserIdToContact = UserIdToContact;
            this.LastMessage = LastMessage;
        }

        public UserContactModel()
        { }
    }
}