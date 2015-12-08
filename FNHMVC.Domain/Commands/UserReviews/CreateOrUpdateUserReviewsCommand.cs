﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateUserReviewsCommand : ICommand
    {
        public virtual long ReviewId { get; set; }
        public virtual bool Active { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Value { get; set; }
        public virtual string Title { get; set; }
        public virtual string Comment { get; set; }
        public virtual FNHMVC.Model.User User { get; set; }
        public virtual FNHMVC.Model.Sale Sale { get; set; }
    }
}
