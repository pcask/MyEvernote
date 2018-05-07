using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.EF;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        Repository<Category> catRepo = new Repository<Category>();
        Repository<EvernoteUser> userRepo = new Repository<EvernoteUser>();
        Repository<Comment> comRepo = new Repository<Comment>();
        Repository<Note> noteRepo = new Repository<Note>();

        public void ListTest()
        {
            // DatabaseContext db = new DatabaseContext();
            // db.Database.CreateIfNotExists(); Bu komut sadece database'in oluşmasını sağlar seed methodunu çalıştırmaz.
            // Seed methodunun çalışması ve database'in oluşması için istediğimiz bir tabloya select atmak yeterli.
            // db.Categories.ToList();

            List<Category> categories = catRepo.List();
            List<Category> filteredCat = catRepo.List(x => x.Id > 5);
        }

        public void InsertTest()
        {
            int result = userRepo.Insert(new EvernoteUser
            {
                Name = "abc",
                Surname = "dfg",
                Username = "abc123",
                Email = "abc.dfg@gmail.com",
                Password = "123",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "pcask"
            });
        }

        public void UpdateTest()
        {
            EvernoteUser user = userRepo.Find(x => x.Name == "abc");

            if (user != null)
            {
                user.Email = "abc.temizlik@gmail.com";
                int result = userRepo.Update(user);
            }
        }

        public void DeleteTest()
        {
            EvernoteUser user = userRepo.Find(x => x.Name == "abc");

            if (user != null)
            {
                int result = userRepo.Delete(user);
            }
        }

        public void CommentInsertTest()
        {
            EvernoteUser user = userRepo.Find(x => x.Id == 1);
            Note note = noteRepo.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text = "Bu bir yorum ekleme testidir",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "pcask",
                Note = note,
                Owner = user
            };

            int result = comRepo.Insert(comment);
        }
    }
}
