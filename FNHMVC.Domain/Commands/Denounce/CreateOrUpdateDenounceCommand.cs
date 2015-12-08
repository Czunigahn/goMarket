using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateDenounceCommand : ICommand
    {
        public CreateOrUpdateDenounceCommand(long denounceId, Model.User userDenouncing, Model.User userToDenounce, Sale saleToDenounce, string reason, string comment, DateTime created, bool tookAction)
        {
            this.DenounceId = denounceId;
            this.UserDenouncing = userDenouncing;
            this.UserToDenounce = userToDenounce;
            this.SaleToDenounce = saleToDenounce;
            this.Reason = reason;
            this.Comment = comment;
            this.Created = created;
            this.TookAction = tookAction;
        }

        public CreateOrUpdateDenounceCommand(Denounce denounce)
        {
            this.DenounceId = denounce.DenounceId;
            this.UserDenouncing = denounce.UserDenouncing;
            this.UserToDenounce = denounce.UserToDenounce;
            this.SaleToDenounce = denounce.SaleToDenounce;
            this.Reason = denounce.Reason;
            this.Comment = denounce.Comment;
            this.Created = denounce.Created;
            this.TookAction = denounce.TookAction;
        }

        public long DenounceId { get; set; }
        public Model.User UserDenouncing { get; set; }
        public Model.User UserToDenounce { get; set; }
        public Sale SaleToDenounce { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public bool TookAction { get; set; }

    }
}