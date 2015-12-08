using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class SaleFormUpdateDataModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Razón del cambio")]
        public int ReasonId;

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Razón del cambio")]
        [StringLength(250, ErrorMessage = "Longitud debe ser de 250 caracteres.")]
        public string ReasonDescription;

        public long SaleId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency, ErrorMessage = "Formato incorrecto.")]
        [Range(0, 5000, ErrorMessage = "Rango incorrecto, debe estar entre 0- 5000")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Inventario")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [DataType(DataType.Currency, ErrorMessage = "Formato incorrecto.")]
        [Range(0, 5000, ErrorMessage = "Rango incorrecto, debe estar entre 0- 5000")]
        public int Quantity { get; set; }

        [Display(Name = "Imagen")]
        public string Picture { get; set; }

        [Display(Name = "Enlace YouTube")]
        public string YouTubeLink { get; set; }

        [Display(Name = "Permite ir a traerlo")]
        public bool TookItHome { get; set; }

        [Display(Name = "Categoria")]
        public long CategoryId { get; set; }

        public string Latitude { get; set; }
        public string Altitude { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public SaleFormUpdateDataModel()
        {

        }

        public SaleFormUpdateDataModel(Sale sale)
        {

            this.Title = sale.Title;
            this.CategoryId = sale.Category.CategoryId;
            this.Cost = sale.Cost;
            this.Description = sale.Description;
            this.Picture = sale.Picture;
            this.Quantity = sale.Quantity;
            this.SaleId = sale.SaleId;
            this.YouTubeLink = sale.YouTubeLink;
            this.ReasonDescription = "";
            this.ReasonId = -1;
            this.Latitude = sale.Latitude;
            this.Altitude = sale.Altitude;
            this.TookItHome = sale.TookItHome;
        }
    }
}