using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FancyImageUploader.Models
{
    public class UploadImageModel
    {
        [Display(Name = "Internet URL")]
        public string Url { get; set; }

        public bool IsUrl { get; set; }

        [Display(Name = "Imagen de Flickr")]
        public string Flickr { get; set; }

        public bool IsFlickr { get; set; }

        [Display(Name = "Desde mi pc")]
        public HttpPostedFileBase File { get; set; }

        public bool IsFile { get; set; }
        
        [Range(0, int.MaxValue)]
        public int X { get; set; }

        [Range(0, int.MaxValue)]
        public int Y { get; set; }

        [Range(1, int.MaxValue)]
        public int Width { get; set; }

        [Range(1, int.MaxValue)]
        public int Height { get; set; }

        public long SaleId { get; set; }
        public int SaleImageType { get; set; }
    }
}