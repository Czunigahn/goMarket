using System.Collections.Generic;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands.SalePendingChange
{
    public class InactivaSalePendingChangeCommand : ICommand
    {
        public List<FNHMVC.Model.SalePendingChange> changes { get; set; }
    }
}