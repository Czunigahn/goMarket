namespace FNHMVC.Model
{
    public class SaleImages
    {
        public SaleImages()
        {
            Activated = true;
            Type = 1;
        }

        public virtual long SaleImagesId { get; set; }
        public virtual string Url { get; set; }
        public virtual bool Activated { get; set; }
        public virtual int Type { get; set; }
        public virtual Model.Sale Sale { get; set; }
        public virtual Model.SalePendingChange SalePendingChange { get; set; }
    }
}