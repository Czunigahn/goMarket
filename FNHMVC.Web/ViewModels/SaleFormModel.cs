using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class SaleFormModel
    {

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

        [Display(Name = "Categoria")]
        public long CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public User User { get; set; }

        public bool Activated { get; set; }

        //[DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }


        [Display(Name = "Permite ir a traerlo")]
        public bool TookItHome { get; set; }

        [Display(Name = "Publicar en facebook")]
        public bool PostOnFacebook { get; set; }

        [Display(Name = "Notificar a mis suscriptores")]
        public bool SendToSuscriptors { get; set; }

        public string Latitude { get; set; }
        public string Altitude { get; set; }

        public bool Spotlight { get; set; }

        public RatingFormModel RatingFormModel { get; set; }

        public Sale Sale { get; set; }

        public List<Sale> SimilarSales { get; set; }

        public bool HasSuscriptors { get; set; }
        //TODO: FIX
        public SaleFormModel(Sale sale, RatingFormModel RatingFormModel, List<Sale> SimilarSales)
        {
            this.Title = sale.Title;
            this.Activated = sale.Activated;
            this.CategoryId = sale.Category.CategoryId;
            this.Cost = sale.Cost;
            this.Created = sale.Created;
            this.Modified = sale.Modified;
            this.Description = sale.Description;
            this.Picture = sale.Picture;
            this.Quantity = sale.Quantity;
            this.SaleId = sale.SaleId;
            this.YouTubeLink = sale.YouTubeLink;
            this.User = sale.User;
            this.SendToSuscriptors = false;
            this.HasSuscriptors = sale.User.Followers.Count > 0;
            this.Latitude = sale.Latitude;
            this.Altitude = sale.Altitude;
            this.TookItHome = sale.TookItHome;
            this.RatingFormModel = RatingFormModel;
            this.Sale = sale;
            this.SimilarSales = SimilarSales;

        }

        public SaleFormModel(SaleFormModel sale)
        {
            this.Title = sale.Title;
            this.Activated = sale.Activated;
            // this.CategoryId = sale.Category.CategoryId;
            this.Cost = sale.Cost;
            this.Created = sale.Created;
            this.Modified = sale.Modified;
            this.Description = sale.Description;
            this.Picture = sale.Picture;
            this.Quantity = sale.Quantity;
            this.SaleId = sale.SaleId;
            this.YouTubeLink = sale.YouTubeLink;
            this.User = sale.User;
            this.SendToSuscriptors = false;
            this.HasSuscriptors = sale.User.Followers.Count > 0;
            this.Latitude = sale.Latitude;
            this.Altitude = sale.Altitude;
            this.TookItHome = sale.TookItHome;
            this.RatingFormModel = sale.RatingFormModel;
            this.Sale = sale.Sale;
            this.SimilarSales = sale.SimilarSales;

        }

        public SaleFormModel()
        {
            SimilarSales = new List<Sale>();
            Sale = new Sale();
            RatingFormModel = new RatingFormModel();
        }
    }
}