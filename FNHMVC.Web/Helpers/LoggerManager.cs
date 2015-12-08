using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
//using log4net;
using NLog;
namespace FNHMVC.Web.Helpers
{
    public class LoggerManager
    {
       // private static readonly ILog logger = LogManager.GetLogger(typeof(LoggerManager));

        private static Logger logger = LogManager.GetCurrentClassLogger();

        //static LoggerManager()
        //{

        //    var root = HostingEnvironment.MapPath("~");



        //    //var log4NetConfigDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        //    var log4NetConfigFilePath = Path.Combine(root, "web.config");

        //    //logger = LogManager.GetLogger(typeof(LoggerManager));

        //   // log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFilePath));
            

        //}

        //public static void LoggerManager(Type logClass)
        //{
        //    logger = LogManager.GetLogger(logClass);
        //}

        public static void WriteError(string text)
        {
            try
            {

                WriteError(text, null);
            }
            catch (Exception)
            { }

        }
        public static void WriteError(Exception ex)
        {
            try
            {
                WriteError("", ex);
            }
            catch (Exception)
            { }
        }

        public static void WriteError(string text, Exception ex)
        {
            try
            {
                logger.Error(text, ex);
            }
            catch (Exception)
            { }
        }




        public static void WriteInfo(string text)
        {
            try
            {
                WriteInfo(text, null);
            }
            catch (Exception)
            { }
        }

        public static void WriteInfo(Exception ex)
        {
            try
            {
                WriteInfo("", ex);
            }
            catch (Exception)
            { }
        }

        public static void WriteInfo(string text, Exception ex)
        {
            try
            {
                logger.Info(text, ex);
            }
            catch (Exception)
            { }
        }






        public static void WriteDebug(string text)
        {
            try
            {
                WriteDebug(text, null);
            }
            catch (Exception)
            { }
        }

        public static void WriteDebug(Exception ex)
        {
            try
            {
                WriteDebug("", ex);
            }
            catch (Exception)
            { }
        }

        public static void WriteDebug(string text, Exception ex)
        {
            try
            {
                logger.Debug(text, ex);
            }
            catch (Exception)
            { }

        }


        public static void WriteAlert(string text)
        {
            try
            {
                WriteAlert(text, null);
            }
            catch (Exception)
            { }
        }


        public static void WriteAlert(Exception ex)
        {
            try
            {

                WriteAlert("", ex);
                //just to trigguer deployer appharbor
                 //just to trigguer deployer appharbor
            }
            catch (Exception)
            { }

        }

        public static void WriteAlert(string text, Exception ex)
        {
            try
            {
                logger.Warn(text, ex);
            }
            catch (Exception)
            { }
        }

    }

}