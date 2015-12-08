using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateGoodDealCommand : ICommand
    {
        public CreateGoodDealCommand(long goodDealId, FNHMVC.Model.User user, Sale sale)
        {
            this.GoodDealId = goodDealId;
            this.User = user;
            this.Sale = sale;
        }

        public CreateGoodDealCommand(GoodDeal goodDeal)
        {
            this.GoodDealId = goodDeal.GoodDealId;
            this.User = goodDeal.User;
            this.Sale = goodDeal.Sale;
        }

        public long GoodDealId { get; set; }
        public FNHMVC.Model.User User { get; set; }
        public Sale Sale { get; set; }
    }
}
