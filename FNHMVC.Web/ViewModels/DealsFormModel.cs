using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class DealsFormModel
    {
        public long SaleId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Precio")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Precio Oferta")]
        public decimal CostDeal { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Inicio Oferta")]
        [DisplayFormat(DataFormatString = "DD/MM/YYYY")]
        public DateTime DateFromDeal { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Fin Oferta")]
        [DisplayFormat(DataFormatString = "DD/MM/YYYY")]
        public DateTime DateToDeal { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Descripción Oferta")]
        public string DescriptionDeal { get; set; }

        public bool HasDeal { get; set; }

        public DealsFormModel(Sale sale)
        {
            this.Title = sale.Title;
            this.Cost = sale.Cost;
            this.Description = sale.Description;
            this.SaleId = sale.SaleId;
            this.HasDeal = sale.HasDeal;

            this.CostDeal = sale.Cost;
            this.DateFromDeal = DateTime.Now;
            this.DateToDeal = DateTime.Now;
        }

        public DealsFormModel(DealsFormModel sale)
        {
            this.Title = sale.Title;
            this.Cost = sale.Cost;
            this.Description = sale.Description;
            this.DescriptionDeal = sale.DescriptionDeal;
            this.SaleId = sale.SaleId;

            this.CostDeal = sale.Cost;
            this.DateFromDeal = DateTime.Now;
            this.DateToDeal = DateTime.Now;
        }

        public DealsFormModel() { }

    }
}