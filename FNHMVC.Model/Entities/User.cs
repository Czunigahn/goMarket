﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using FNHMVC.Core.Common;


namespace FNHMVC.Model
{

    public class User
    {
        public User()
        {
            DateCreated = DateTime.Now;
            Activated = false;
            Locked = false;
        }

        public virtual long UserId { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int Age { get; set; }
        public virtual bool Genre { get; set; }
        public virtual string Country { get; set; }
        public virtual string PaypalAccount { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual bool Activated { get; set; }

        public virtual bool Locked { get; set; }
        public virtual string Picture { get; set; }
        public virtual string About { get; set; }
        public virtual int Rate { get; set; }


        public virtual int RoleId { get; set; }
        public virtual IList<Sale> Sales { get; set; }
        public virtual IList<Follow> Followers { get; set; }
        public virtual IList<UserReviews> Reviews { get; set; }
        public virtual IList<UserAddress> UserAddress { get; set; }
        public virtual IList<UserInbox> UserInbox { get; set; }
        public virtual IList<Cupon> Cupons { get; set; }

        public virtual string Password
        {
            set { PasswordHash = Md5Encrypt.Md5EncryptPassword(value); }
        }

        internal static string GenerateRandomString()
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(25 * random.NextDouble() + 75)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public virtual string ResetPassword()
        {
            var newPass = GenerateRandomString();
            Password = newPass;
            return newPass;
        }

        public virtual string DisplayName
        {
            get { return FirstName; }
        }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }

    }
}
