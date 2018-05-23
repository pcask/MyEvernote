using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();

        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Note note = noteManager.QueryableList().Include("Comments").FirstOrDefault(x => x.Id == id.Value);

            if (note == null)
                return HttpNotFound();

            return PartialView("_PartialComments", note.Comments);
        }

        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = commentManager.Find(c => c.Id == id.Value);

            if (comment == null)
                return new HttpNotFoundResult();

            comment.Text = text;

            if (commentManager.Update(comment) > 0)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Comment comment = commentManager.Find(c => c.Id == id.Value);

            if (comment == null)
                return new HttpNotFoundResult();

            if (commentManager.Delete(comment) > 0)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        // Ajax ile gönderdiğimiz text isimli parametre burada Comment nesnesinin text property'sine otomatik olarak atanır.
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteId) 
        {
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                if (noteId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Note note = noteManager.Find(n => n.Id == noteId.Value);

                if (note == null)
                    return new HttpNotFoundResult();

                comment.Note = note;
                comment.Owner = CurrentSession.User;

                if (commentManager.Insert(comment) > 0)
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

    }
}