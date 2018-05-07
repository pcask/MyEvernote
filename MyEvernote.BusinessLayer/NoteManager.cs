using MyEvernote.DataAccessLayer.EF;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> noteRepo = new Repository<Note>();

        public List<Note> GetNotes()
        {
            return noteRepo.List();
        }

        public List<Note> GetNotes(int skipCount, int takeCount)
        {
            return noteRepo.QueryableList().OrderByDescending(x => x.ModifiedOn).Skip(skipCount).Take(takeCount).ToList();
        }

        public List<Note> GetNotesByCategoryId(int id)
        {
            return noteRepo.QueryableList().Where(n => n.CategoryId == id).OrderByDescending(m => m.ModifiedOn).ToList();
        }
        public List<Note> GetNotesByCategoryId(int id, int skipCount, int takeCount)
        {
            return noteRepo.QueryableList().Where(n => n.CategoryId == id).OrderByDescending(m => m.ModifiedOn).Skip(skipCount).Take(takeCount).ToList();
        }

        public List<Note> GetNotesByMostLiked(int skipCount, int takeCount)
        {
            return noteRepo.QueryableList().OrderByDescending(n => n.LikeCount).Skip(skipCount).Take(takeCount).ToList();
        }
    }
}
