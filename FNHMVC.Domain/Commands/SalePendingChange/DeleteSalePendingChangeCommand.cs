using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands.SalePendingChange
{
    public class DeleteSalePendingChangeCommand : ICommand
    {
        public long SalePendingChangeId { get; set; }
    }
}
