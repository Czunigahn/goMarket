using System;

namespace FNHMVC.Model
{
    public class Denounce
    {
        public virtual long DenounceId { get; set; }
        public virtual User UserDenouncing { get; set; }
        public virtual User UserToDenounce { get; set; }
        public virtual Sale SaleToDenounce { get; set; }
        public virtual string Reason { get; set; }
        public virtual string Comment { get; set; }
        public virtual bool TookAction { get; set; }
        public virtual DateTime Created { get; set; }
    }
}