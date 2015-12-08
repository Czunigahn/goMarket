using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FlickrNet;
using System.Text;
using System.Collections;
using FNHMVC.Model;
using com.mosso.cloudfiles;
using com.mosso.cloudfiles.domain;
using System.IO;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;

namespace FNHMVC.Web.Helpers
{

    public class FileUploadManager
    {
        string username;
        string api_key;
        string chosenContainer;
        string UrlStorage;
        string UrlAuth;
        string RackIsOnline;
        public FileUploadManager()
        {
            try
            {
                username = ConfigurationManager.AppSettings["RACK_USER"].ToString();
                api_key = ConfigurationManager.AppSettings["RACK_API_KEY"].ToString();
                chosenContainer = ConfigurationManager.AppSettings["RACK_CONTAINER"].ToString();
                UrlStorage = ConfigurationManager.AppSettings["RACK_URL_STORAGE"].ToString();
                UrlAuth = ConfigurationManager.AppSettings["RACK_URL_AUTH"].ToString();
                RackIsOnline = ConfigurationManager.AppSettings["RACK_ONLINE"].ToString();
            }
            catch (Exception ex)
            {
                LoggerManager.WriteAlert(ex);
            }
        }

        public IList<SaleImages> UploadImages(Sale sale, SaleImageType type, string images)
        {
            //var list = new List<SaleImages>();
            sale.SaleImages = new List<SaleImages>();

            if (images == null || images.Trim().Length <= 0)
                return sale.SaleImages;

            try
            {
              

                //var containerInfo = connection.GetContainerInformation(chosenContainer);

                //if (containerInfo == null)
                //{
                //    connection.CreateContainer(chosenContainer);
                //    //return list;
                //}

                var imagesList = images.Split(';');

                //var cmd = new CreateOrUpdateSaleCommand(sale, false);

                foreach (string imgPath in imagesList)
                {

                    string path = "";


                    if (RackIsOnline == "1")
                    {
                        UserCredentials userCreds = new UserCredentials(new Uri(UrlAuth), username, api_key, null, null);
                        Connection connection = new com.mosso.cloudfiles.Connection(userCreds);

                        string extension = System.IO.Path.GetExtension(imgPath);
                        string name = System.IO.Path.GetFileNameWithoutExtension(imgPath);
                        string tempName = System.IO.Path.GetRandomFileName();
                        tempName = tempName + extension;

                        FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                        connection.PutStorageItem(chosenContainer, fileStream, tempName);

                        path = UrlStorage + tempName;
                    }
                    else
                        path = imgPath;

                    var SaleImages = new SaleImages();
                    SaleImages.Activated = true;
                    SaleImages.SaleImagesId = 0;
                    SaleImages.SalePendingChange = null;
                    SaleImages.Sale = sale;
                    SaleImages.Type = (int)type;
                    SaleImages.Url = path;


                    sale.SaleImages.Add(SaleImages);


                }
            }
            catch (Exception ex)
            {
                LoggerManager.WriteAlert(ex);
            }

            return sale.SaleImages;
        }
    }


}