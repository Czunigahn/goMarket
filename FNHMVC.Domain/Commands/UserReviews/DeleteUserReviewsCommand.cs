using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteUserReviewsCommand : ICommand
    {
        public virtual long ReviewId { get; set; }
    }
}
