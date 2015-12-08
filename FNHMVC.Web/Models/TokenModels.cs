using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNHMVC.Web.Models
{
    public class TokenResetPasswordMail
    {
        public string Email { get; set; }
        public long UserId { get; set; }
        public string ConfirmationToken { get; set; }
        public int Action { get; set; }
        public bool Activated { get; set; }
    }

    public class TokenVerificationMail
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int TokenId { get; set; }
        public long UserId { get; set; }
        public string ConfirmationToken { get; set; }
        public int Action { get; set; }
        public bool Activated { get; set; }
    }
}
