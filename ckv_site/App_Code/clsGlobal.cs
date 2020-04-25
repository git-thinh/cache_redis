using System;
using System.Web.Routing;

namespace ckv_site
{
    public class clsGlobal : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var startupProperties = SimpleOwinAspNetHandler.GetStartupProperties();
            RouteTable.Routes.Add(new Route("helloworld", new SimpleOwinAspNetRouteHandler(Helloworld.OwinApp())));

            //SimpleOwinAspNetRouteHandler is capable of auto handling IEnumerable<Func<AppFunc,AppFunc>>
            RouteTable.Routes.Add(new Route("middlewareapps", new SimpleOwinAspNetRouteHandler(MiddlewareApps.OwinApps())));

            RouteTable.Routes.Add(new Route("SimpleOwinApp/{*pathInfo}", new SimpleOwinAspNetRouteHandler(SimpleOwinApp.OwinApp(), "SimpleOwinApp")));

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //clsApp._init();
            //clsRouter.execute_api(Request, Response);
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