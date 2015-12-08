using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateCuponCommand : ICommand
    {
        public CreateOrUpdateCuponCommand(long cuponId, Model.User user, string cuponName, double discount, DateTime created, bool isActive, int timesUsed)
        {
            this.CuponId = CuponId;
            this.User = user;
            this.CuponName = cuponName;
            this.Discount = discount;
            this.Created = created;
            this.IsActive = isActive;
            this.TimesUsed = timesUsed;
        }


        public CreateOrUpdateCuponCommand(Cupon cupon)
        {
            this.CuponId = cupon.CuponId;
            this.CuponName = cupon.CuponName;
            this.User = cupon.User;
            this.Discount = cupon.Discount;
            this.IsActive = cupon.IsActive;
            this.TimesUsed = cupon.TimesUsed;
            this.Created = cupon.Created;
        }
        public CreateOrUpdateCuponCommand()
        {
        }

        public long CuponId { get; set; }
        public Model.User User { get; set; }
        public string CuponName { get; set; }
        public double Discount { get; set; }
        public int TimesUsed { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
    }
}
