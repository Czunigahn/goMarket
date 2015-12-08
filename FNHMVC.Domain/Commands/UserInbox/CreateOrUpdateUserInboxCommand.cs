using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateUserInboxCommand : ICommand
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

        public CreateOrUpdateUserInboxCommand(long UserInboxId, DateTime DateCreate, DateTime DateRead, bool WasRead, bool Activated, bool SentEmail, string Subject, string Message, string LastMessage, Model.User User, Model.User Seller)
        {
            this.UserInboxId = UserInboxId;
            this.DateCreate = DateCreate;


            this.DateRead = DateRead;

            this.WasRead = WasRead;
            this.Activated = Activated;
            this.SentEmail = SentEmail;
            this.Subject = Subject;
            this.Message = Message;
            this.User = User;
            this.Seller = Seller;

            this.LastMessage = LastMessage;
        }

        //public CreateOrUpdateUserInboxCommand() { }

        public CreateOrUpdateUserInboxCommand(FNHMVC.Model.UserInbox Inbox)
        {
            this.UserInboxId = Inbox.UserInboxId;
            this.DateCreate = Inbox.DateCreate;


            this.DateRead = Inbox.DateRead;

            this.WasRead = Inbox.WasRead;
            this.Activated = Inbox.Activated;
            this.SentEmail = Inbox.SentEmail;
            this.Subject = Inbox.Subject;
            this.Message = Inbox.Message;
            this.User = Inbox.User;
            this.Seller = Inbox.Seller;
            this.LastMessage = Inbox.LastMessage;
        }
    }
}
