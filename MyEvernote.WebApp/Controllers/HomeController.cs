using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModels;
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
        private NoteManager nm = new NoteManager();
        private CategoryManager cm = new CategoryManager();
        private EvernoteUserManager eum = new EvernoteUserManager();

        public ActionResult Index()
        {
            return View(nm.QueryableList().OrderByDescending(n => n.ModifiedOn).Take(6).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Category cat = cm.GetCategoryById(id.Value, true);
            if (cat == null)
                return HttpNotFound();

            ViewBag.Baslik = cat.Title + " Notları";
            return View("Index", nm.QueryableList().Where(n => n.CategoryId == id.Value).OrderByDescending(m => m.ModifiedOn).Take(6).ToList());
        }

        public ActionResult MostLiked()
        {
            ViewBag.Baslik = "En Beğenilen Notlar";
            return View("Index", nm.QueryableList().OrderByDescending(n => n.LikeCount).Take(6).ToList());
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

                OkViewModel okViewModel = new OkViewModel();

                okViewModel.Items.Add("Kayıt işleminiz başarılı bir şekilde gerçekleştirilmiştir");
                okViewModel.Items.Add("E-posta adresinize gönderilen aktivasyon linki ile hesabınızı aktifleştirebilirsiniz");

                return View("Ok", okViewModel);
            }

            return View(model);
        }

        public ActionResult UserActivate(Guid activateID)
        {
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> blr = eum.ActivateUser(activateID);

            if (blr.Result != null)
            {
                if (blr.Informations.Count > 0)
                {
                    InfoViewModel model = new InfoViewModel();

                    blr.Informations.ForEach(x =>
                    {
                        model.Items.Add(x.Value);
                    });

                    return View("Info", model);
                }
                else
                {
                    OkViewModel model = new OkViewModel();

                    model.Items.Add("Hesap aktivasyon işleminiz başarılı bir şekilde gerçekleştirilmiştir.");

                    return View("Ok", model);
                }
            }
            else
            {
                ErrorViewModel model = new ErrorViewModel();

                blr.Errors.ForEach(x =>
                {
                    model.Items.Add(x.Value);
                });

                return View("Error", model);
            }
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

                if (blr.Informations.Count > 0)
                {
                    List<string> infoList = new List<string>();

                    blr.Informations.ForEach(x => infoList.Add(x.Value));

                    ViewBag.InfoList = infoList;
                }

                if (blr.Errors.Count > 0)
                {
                    blr.Errors.ForEach(x => ModelState.AddModelError("", x.Value));

                    return View(model);
                }

                CurrentSession.Set<EvernoteUser>("login", blr.Result);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = CurrentSession.User;

            if (currentUser != null)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> blr = eum.GetUserById(currentUser.Id);

                if (blr.Errors.Count > 0)
                {
                    ErrorViewModel errorModel = new ErrorViewModel();

                    blr.Errors.ForEach(x =>
                    {
                        errorModel.Items.Add(x.Value);
                    });

                    return View("Error", errorModel);
                }
                else
                {
                    return View(blr.Result);
                }
            }

            return RedirectToAction("Login");
        }

        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = CurrentSession.User;

            if (currentUser != null)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> blr = eum.GetUserById(currentUser.Id);

                if (blr.Errors.Count > 0)
                {
                    ErrorViewModel errorModel = new ErrorViewModel();

                    blr.Errors.ForEach(x =>
                    {
                        errorModel.Items.Add(x.Value);
                    });

                    return View("Error", errorModel);
                }
                else
                {
                    return View(blr.Result);
                }
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase profileImage)
        {
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                if (profileImage != null &&
                (profileImage.ContentType == "image/jpeg" ||
                profileImage.ContentType == "image/jpg" ||
                profileImage.ContentType == "image/png"))
                {
                    string fileName = $"user_{model.Id}.{profileImage.ContentType.Split('/')[1]}";

                    profileImage.SaveAs(Server.MapPath($"~/Images/Users/{fileName}"));
                    model.ProfileImageFileName = fileName;
                }

                BusinessLayerResult<EvernoteUser> blr = eum.UpdateProfile(model);

                if (blr.Errors.Count > 0)
                {
                    ErrorViewModel errorModel = new ErrorViewModel()
                    {
                        RedirectingUrl = "/Home/EditProfile",
                        RedirectingPageName = "Profil Düzenle",
                        RedirectingTimeout = 500
                    };

                    blr.Errors.ForEach(x => errorModel.Items.Add(x.Value));

                    return View("Error", errorModel);
                }

                CurrentSession.Set<EvernoteUser>("login", blr.Result); // Profil güncellendiği için session da güncellenmeli

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        public ActionResult ChangePass()
        {
            return View(new ChangePasswordVO());
        }

        [HttpPost]
        public ActionResult ChangePass(ChangePasswordVO model)
        {
            if (ModelState.IsValid)
            {
                EvernoteUser currentUser = CurrentSession.User;
                if (currentUser != null)
                {
                    BusinessLayerResult<EvernoteUser> blr = eum.ChangePass(currentUser, model);

                    if (blr.Errors.Count > 0)
                    {
                        KeyValuePair<ErrorCode, string> notFound = blr.Errors.Find(x => x.Key == ErrorCode.UserNotFound);

                        if (!String.IsNullOrEmpty(notFound.Value))
                        {
                            ErrorViewModel errorModel = new ErrorViewModel();

                            blr.Errors.ForEach(x => errorModel.Items.Add(x.Value));

                            return View("Error", errorModel);
                        }
                        else
                        {
                            blr.Errors.ForEach(x => ModelState.AddModelError("", x.Value));

                            return View(model);
                        }
                    }

                    Session.Clear();

                    OkViewModel okModel = new OkViewModel();
                    okModel.Items.Add("Şifre değiştirme işleminiz başarılı bir şekilde gerçekleştirilmiştir");
                    okModel.Items.Add("Lütfen devam etmek için giriş yapınız");

                    return View("Ok", okModel);
                }
            }

            return View(model);

        }

        public ActionResult DeleteProfile()
        {
            EvernoteUser currentUser = CurrentSession.User;

            if (currentUser != null)
            {
                BusinessLayerResult<EvernoteUser> blr = eum.RemoveUserById(currentUser.Id);

                if (blr.Errors.Count > 0)
                {
                    ErrorViewModel errorModel = new ErrorViewModel()
                    {
                        RedirectingPageName = "Profilim",
                        RedirectingUrl = "/Home/ShowProfile"
                    };

                    blr.Errors.ForEach(x => errorModel.Items.Add(x.Value));

                    return View("Error", errorModel);

                }

                Session.Clear();

                return RedirectToAction("Index");
            }

            return View();

        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}