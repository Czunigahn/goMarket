using System.ComponentModel.DataAnnotations;

namespace FNHMVC.Web.ViewModels
{
    public class CuponCreateFormModel
    {
        public long CuponId { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        [Display(Name = "Codigo Cupón")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descuento requerido")]
        [Display(Name = "Descuento")]
        public double Discount { get; set; }

        [Display(Name = "Activado?")]
        public bool Activado { get; set; }
    }
}