using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        NoteManager nm = new NoteManager();
        CategoryManager cm = new CategoryManager();
        EvernoteUserManager eum = new EvernoteUserManager();

        public ActionResult Index()
        {
            // CategoryController üzerinden gelen view talebi ile gönderilen model;
            //if (TempData["notesByCategory"] != null)
            //    return View(TempData["notesByCategory"] as List<Note>);

            List<Note> notes = nm.GetNotes(0, 6);

            return View(notes);
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Burada kategorinin kendisini direk çağırırsak kategori altındaki bütün notlarda ram'e çıkmış olacak. Hoş değil
            // Category cat = cm.GetCategoryById(id.Value);

            Category cat = cm.GetCategoryById(id.Value, true);
            if (cat == null)
                return HttpNotFound();

            ViewBag.Baslik = cat.Title + " Notları";
            return View("Index", nm.GetNotesByCategoryId(cat.Id, 0, 6));
        }


        public ActionResult MostLiked()
        {
            ViewBag.Baslik = "En Beğenilen Notlar";
            return View("Index", nm.GetNotesByMostLiked(0, 6));
        }

        public ActionResult About()
        {
            ViewBag.Baslik = "Hakkımızda";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVO model)
        {
            if (model.ContractAccepted == false)
                ModelState.AddModelError("ContractAccepted", "Devam etmek için hizmet sözleşmesini kabul etmelisin.");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> blr = eum.RegisterUser(model);

                if (blr.Errors.Count > 0)
                {
                    blr.Errors.ForEach(x => ModelState.AddModelError("", x.Value));
                    return View(model);
                }

                return RedirectToAction("RegisterOk");
            }

            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activateID)
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVO model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> blr = eum.LoginUser(model);

                if (blr.Errors.Count > 0)
                {
                    blr.Errors.ForEach(x => ModelState.AddModelError("", x.Value));

                    if (blr.Errors.Count(x => x.Key == ErrorCode.UserIsNotActive) > 0)
                    {
                        ViewBag.Info = "Lütfen e-posta adresinizi kontrol ediniz.";
                        // Burada kullanıcının e-mail adresine yeni bir aktivasyon mail'i gönderilebilir.
                    }

                    return View(model);
                }

                Session["login"] = blr.Result;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}