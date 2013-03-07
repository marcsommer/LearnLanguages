using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnLanguages.Mvc4.Controllers
{
  [AllowAnonymous]
  public class HomeController : Controller
  {
    [AllowAnonymous]
    public ActionResult Index()
    {
      //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

      return View();
    }

    [AllowAnonymous]
    public ActionResult About()
    {
      ViewBag.Message = "Your app description page.";

      return View();
    }

    [AllowAnonymous]
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    
  }
}
