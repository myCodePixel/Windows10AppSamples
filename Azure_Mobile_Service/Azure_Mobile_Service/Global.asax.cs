using System.Web.Http;
using System.Web.Routing;

namespace Azure_Mobile_Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register();
        }
    }
}