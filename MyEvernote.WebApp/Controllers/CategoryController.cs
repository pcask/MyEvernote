using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        // TempData ile farklı controller'lar arası model taşıma;
        //public ActionResult Select(int? id)
        //{
        //    CategoryManager cm = new CategoryManager();

        //    if (id == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    Category cat = cm.GetCategoryById(id.Value);

        //    if (cat == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

        //    TempData["notesByCategory"] = cat.Notes;
        //    return RedirectToAction("Index", "Home");
        //}
    }
}