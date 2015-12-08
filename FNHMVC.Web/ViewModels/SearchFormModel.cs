using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class SearchFormModel
    {

        [Display(Name = "Desde")]
        public decimal StartPrice { get; set; }

        [Display(Name = "Hasta")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency, ErrorMessage = "Formato incorrecto.")]
        [Range(0, 99999, ErrorMessage = "Rango incorrecto, debe estar entre 0- 99999")]
        public decimal EndPrice { get; set; }

        [Display(Name = "Desde")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency, ErrorMessage = "Formato incorrecto.")]
        [Range(0, 99999, ErrorMessage = "Rango incorrecto, debe estar entre 0- 99999")]
        public int Rating { get; set; }

        [Display(Name = "Contiene")]
        public string GeneralFilter { get; set; }

        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }
        public IList<Category> Categories { get; set; }

        [Display(Name = "Desde")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency, ErrorMessage = "Formato incorrecto.")]
        [Range(0, 99999, ErrorMessage = "Rango incorrecto, debe estar entre 0- 99999")]
        public int StartGoodDeal { get; set; }

        [Display(Name = "Hasta")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency, ErrorMessage = "Formato incorrecto.")]
        [Range(0, 99999, ErrorMessage = "Rango incorrecto, debe estar entre 0- 99999")]
        public int EndGoodDeal { get; set; }

        [Display(Name = "Desde")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha incorrecto.")]
        public DateTime StartDate { get; set; }

        
        [Display(Name = "Hasta")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha incorrecto.")]
        
        public DateTime EndDate { get; set; }


        public bool AdvanceSearch { get; set; }
    }
}