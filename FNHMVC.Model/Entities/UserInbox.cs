using System;

namespace FNHMVC.Model
{
    public class UserInbox
    {
        public virtual long UserInboxId { get; set; }
        public virtual DateTime DateCreate { get; set; }
        public virtual DateTime DateRead { get; set; }
        public virtual bool WasRead { get; set; }
        public virtual bool Activated { get; set; }
        public virtual bool SentEmail { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Message { get; set; }
        public virtual string LastMessage { get; set; }
        public virtual FNHMVC.Model.User User { get; set; }
        public virtual FNHMVC.Model.User Seller { get; set; }

    }
}