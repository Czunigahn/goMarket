using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateCartCommand : ICommand
    {
        public CreateOrUpdateCartCommand(long cartId, Model.User user, Sale sale, int quantity, DateTime created, Cupon cupon)
        {
            this.CartId = cartId;
            this.User = user;
            this.Sale = sale;
            this.Quantity = quantity;
            this.Created = created;
            this.Cupon = cupon;
        }

        public CreateOrUpdateCartCommand(Cart cart)
        {
            this.CartId = cart.CartId;
            this.User = cart.User;
            this.Sale = cart.Sale;
            this.Quantity = cart.Quantity;
            this.Created = cart.Created;
            this.Cupon = cart.Cupon;
        }

        public long CartId { get; set; }
        public Model.User User { get; set; }
        public Sale Sale { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }
        public Cupon Cupon { get; set; }
    }
}
