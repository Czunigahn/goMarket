using System;

namespace FNHMVC.Model
{
    public class Cupon
    {
        public virtual long CuponId { get; set; }
        public virtual User User { get; set; }
        public virtual string CuponName { get; set; }
        public virtual int TimesUsed { get; set; }
        public virtual double Discount { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual bool IsActive { get; set; }
    }
}