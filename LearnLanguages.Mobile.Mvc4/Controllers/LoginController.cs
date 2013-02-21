using LearnLanguages.Business.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LearnLanguages.Mobile.Mvc4.Controllers
{
  public class LoginController : Controller
  {
    //
    // GET: /Login/

    public ActionResult Index()
    {
      return View();
    }

    public async Task<ActionResult> Login()
    {
      await UserPrincipal.LoginAsync("user", "password");
      
      return View();
    }

    public ActionResult Logout()
    {
      UserPrincipal.Logout();

      return View();
    }

    //
    // GET: /Login/Details/5

    public ActionResult Details(int id)
    {
      return View();
    }

    //
    // GET: /Login/Create

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Login/Create

    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      try
      {
        // TODO: Add insert logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    //
    // GET: /Login/Edit/5

    public ActionResult Edit(int id)
    {
      return View();
    }

    //
    // POST: /Login/Edit/5

    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    //
    // GET: /Login/Delete/5

    public ActionResult Delete(int id)
    {
      return View();
    }

    //
    // POST: /Login/Delete/5

    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
