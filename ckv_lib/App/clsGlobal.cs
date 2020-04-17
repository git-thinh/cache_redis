using System;

namespace ckv_lib
{
    public class clsGlobal : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            if (_CONFIG.PATH_ROOT.Length == 0) _CONFIG.PATH_ROOT = Server.MapPath("~/");
            clsApp._init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            clsApp._init();
            clsRouter.execute_api(Request, Response);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}