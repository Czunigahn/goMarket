using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateTokenCommand : ICommand
    {
        public long TokenId { get; set; }
        public long UserId { get; set; }
        public string ConfirmationToken { get; set; }
        public int Action { get; set; }
        public bool Activated { get; set; }
    }
}
