using LearnLanguages.Business.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace LearnLanguages.Mvc4
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      ModelBinders.Binders.DefaultBinder = new Csla.Web.Mvc.CslaModelBinder();

      AreaRegistration.RegisterAllAreas();

      WebApiConfig.Register(GlobalConfiguration.Configuration);

      //These filters set up an "implicit deny" type of structure. 
      //Now, for any anonymous access, you have to add [AllowAnonymous]
      GlobalFilters.Filters.Add(new HandleErrorAttribute());
      GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      //BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
      if (Mvc4Helper.CurrentUserIsAuthenticated())
      {
        UserPrincipal.Load(Csla.ApplicationContext.User.Identity.Name);
      }
    }


  }
}