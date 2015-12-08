using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FNHMVC.Web.ViewModels
{
    public class LogOnFormModel
    {
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Correo")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "Recordarme")]
            public bool RememberMe { get; set; }
      
    }
}