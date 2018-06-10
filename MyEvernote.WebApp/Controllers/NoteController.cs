using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;

namespace MyEvernote.WebApp.Controllers
{
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikedManager likedManager = new LikedManager();

        [MyAuthorization]
        public ActionResult Index()
        {
            var notes = noteManager.QueryableList().Include("Category").Include("Owner").Where(
             n => n.Owner.Id == CurrentSession.User.Id).OrderByDescending(
             n => n.ModifiedOn);

            return View(notes.ToList());
        }

        [MyAuthorization]
        public ActionResult MyLikedNotes()
        {
            var notes = likedManager.QueryableList().Include("LikedUser").Include("Note").Where(
                x => x.LikedUser.Id == CurrentSession.User.Id).Select(
                x => x.Note).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

            return View("Index", notes.ToList());
        }

        [MyAuthorization]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = noteManager.Find(n => n.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [MyAuthorization]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        [MyAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [MyAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = noteManager.Find(n => n.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [MyAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Note db_note = noteManager.Find(n => n.Id == note.Id);

                db_note.IsDraft = note.IsDraft;
                db_note.CategoryId = note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;

                noteManager.Update(db_note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [MyAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(n => n.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        [MyAuthorization]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(n => n.Id == id);
            noteManager.Delete(note);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetLiked(int[] noteIdList)
        {
            if (noteIdList == null || noteIdList.Length <= 0)
                return null;

            // Örnek sorgu - 1
            //List<int> likedNoteIdList = (from liked in likedManager.QueryableList()
            //                            where noteIdList.Contains(liked.Note.Id) && liked.LikedUser.Id == CurrentSession.User.Id
            //                            select liked.Note.Id).ToList();

            // Örnek Sorgu - 2
            List<int> likedNoteIdList = likedManager.List(
                x => x.LikedUser.Id == CurrentSession.User.Id && noteIdList.Contains(x.Note.Id)).Select(
                x => x.Note.Id).ToList();

            return Json(new { result = likedNoteIdList }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SetLikeStatus(int noteId, bool liking)
        {
            Liked liked = likedManager.Find(x => x.Note.Id == noteId && x.LikedUser.Id == CurrentSession.User.Id);

            Note note = noteManager.Find(x => x.Id == noteId);

            int result = 0;

            if (liked != null && liking == false)
            {
                result = likedManager.Delete(liked);
            }
            else if (liked == null && liking == true)
            {
                result = likedManager.Insert(new Liked
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }

            if (result > 0)
            {
                if (liking == true)
                    note.LikeCount++;
                else
                    note.LikeCount--;

                if (noteManager.Update(note) > 0)
                    return Json(new { errorMessage = String.Empty, hasError = false, result = note.LikeCount });

            }

            return Json(new { errorMessage = "Beğenme işlemi başarısız!", hasError = true, result = note.LikeCount });

        }

        public ActionResult ShowNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(n => n.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialNoteText", note);
        }

    }
}
