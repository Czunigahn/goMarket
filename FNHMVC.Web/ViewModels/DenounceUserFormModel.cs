using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FNHMVC.Web.ViewModels
{
    public class DenounceUserFormModel
    {

        public long DenounceId { get; set; }

        public string UserName { get; set; }

        public string Reason { get; set; }

        public IEnumerable<SelectListItem> Reasons { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Comentario al Admin")]
        public string Comment { get; set; }

        public DenounceUserFormModel(string userName)
        {
            this.UserName = userName;
        }

        public DenounceUserFormModel() { }
    }
}