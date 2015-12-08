using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNHMVC.Model
{
    public class Token
    {
        public virtual long TokenId { get; set; }
        public virtual long UserId { get; set; }
        public virtual string ConfirmationToken { get; set; }
        public virtual int Action { get; set; }
        public virtual bool Activated { get; set; }
    }
}
