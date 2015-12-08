using System.ComponentModel.DataAnnotations;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class UserEditFormModel
    {

        public long UserId { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Edad")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Genero")]
        public bool Genre { get; set; }

        [Required]
        [Display(Name = "Pais")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Cuenta Paypal")]
        public string PaypalAccount { get; set; }



        [Display(Name = "Fotografia")]
        public string Picture { get; set; }



        [Display(Name = "Acerca de mi")]
        public string About { get; set; }

        [Display(Name = "Olvidó su contraseńa?")]
        public bool ForgotPassword { get; set; }

        public UserEditFormModel()
        {

        }

        public UserEditFormModel(User user)
        {
            this.UserId = user.UserId;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Age = user.Age;
            this.Genre = user.Genre;
            this.Country = user.Country;
            this.PaypalAccount = user.PaypalAccount;
            this.Picture = user.Picture;
            this.About = user.About;
        }
    }
}