


* Install: 
 
 public class Global : System.Web.HttpApplication
 {
        protected void Application_Start(object sender, EventArgs e)
        {
            clsApp._init(Server.MapPath("~/"));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            clsApp._init(Server.MapPath("~/"));
            clsRouter.set_router(Request, Response);
        }
 }