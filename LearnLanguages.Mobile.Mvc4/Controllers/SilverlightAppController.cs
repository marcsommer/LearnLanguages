﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnLanguages.Mobile.Mvc4.Controllers
{
  public class SilverlightAppController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

      return View();
    }

  }
}
