using System.Web.Mvc;
using BootstrapSupport;

namespace BootstrapMvcSample.Controllers
{
    public class BootstrapBaseController : Controller
    {        
        public void Attention(string message)
        {
            TempData.Clear();
            TempData.Add(Alerts.ATTENTION, message);
           // FNHMVC.Web.Helpers.LoggerManager.WriteAlert(message);
        }

        public void Success(string message)
        {
            TempData.Clear();
            TempData.Add(Alerts.SUCCESS, message);
            //FNHMVC.Web.Helpers.LoggerManager.WriteInfo(message);
        }

        public void Information(string message)
        {
            TempData.Clear();
            TempData.Add(Alerts.INFORMATION, message);
            //FNHMVC.Web.Helpers.LoggerManager.WriteDebug(message);
        }

        public void Error(string message)
        {
            TempData.Clear();
            TempData.Add(Alerts.ERROR, message);
            
            FNHMVC.Web.Helpers.LoggerManager.WriteError(message);
        }
    }
}
