using System.ComponentModel.DataAnnotations;

namespace FNHMVC.Web.ViewModels
{
    public class CuponAddFormModel
    {
        [Required(ErrorMessage = "Nombre requerido")]
        [Display(Name = "Codigo Cup�n")]
        public string Name { get; set; }
    }
}