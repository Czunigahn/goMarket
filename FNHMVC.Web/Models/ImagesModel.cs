using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FNHMVC.Model;

namespace FancyImageUploader.Models
{
    public class ImagesModel
    {
        //public ImagesModel()
        //{
        //    Images = new List<string>();
        //}

        //public List<string> Images { get; set; }
        public List<SaleImages> SaleImages { get; set; }
        public List<SaleImages> SaleImagesBanner { get; set; }
        //public long SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}