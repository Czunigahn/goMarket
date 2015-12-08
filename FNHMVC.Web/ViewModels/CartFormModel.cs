using System.ComponentModel.DataAnnotations;

namespace FNHMVC.Web.ViewModels
{
    public class CartFormModel
    {
        public long SaleId { get; set; }

        [Required(ErrorMessage = "Cantidad requerida")]
        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }

        //TODO: FIX
        public CartFormModel(int saleId)
        {
            this.SaleId = saleId;
            Quantity = 1;
        }

        public CartFormModel()
        {
            Quantity = 1;
        }
    }
}