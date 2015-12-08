using System;
using System.Collections.Generic;

namespace FNHMVC.Model
{
    public class UserWishList
    {
        public virtual long UserWishListId { get; set; }
        public virtual DateTime DateCreate { get; set; }
        public virtual bool Activated { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual User User { get; set; }
        public virtual Sale Sale { get; set; }
    }
}