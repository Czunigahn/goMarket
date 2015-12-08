using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FNHMVC.Web.ViewModels
{
    public class DenounceSaleFormModel
    {

        public long DenounceId { get; set; }

        public string SaleName { get; set; }

        public string Reason { get; set; }

        public IEnumerable<SelectListItem> Reasons { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Comentario al Admin")]
        public string Comment { get; set; }

        public DenounceSaleFormModel(string saleName)
        {
            this.SaleName = saleName;
        }

        public DenounceSaleFormModel() { }
    }
}