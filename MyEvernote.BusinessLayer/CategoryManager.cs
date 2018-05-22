using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.DataAccessLayer.EF;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        // Veri tabanından ilişkili bir veri silmek için önce o verinin ilişkili olduğu diğer veriler silinmelidir. Burada kategori silmek için aşağıdaki adımlar izleniyor: Önce kategoriye ait notlar silinmelidir, Notlarında silinebilmesi için notların ilişkili olduğu yorumlar ve beğeniler silinmelidir.

        //public override int Delete(Category category)
        //{
        //    NoteManager noteManager = new NoteManager();
        //    CommentManager commentManager = new CommentManager();
        //    LikedManager likedManager = new LikedManager();

        //    // Kategori ile ilişkili notların silinmesi gerekiyor.
        //    foreach (Note note in category.Notes.ToList()) // Silme işlemi yapılacağı için ToList methodu çağrılıyor her defasında
        //    {

        //        // Not ile ilişkili yorumların silinmesi gerekiyor.
        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            commentManager.Delete(comment);
        //        }

        //        // Not ile ilişkili beğenilerin silinmesi
        //        foreach (Liked liked in note.Likes.ToList())
        //        {
        //            likedManager.Delete(liked);
        //        }

        //        noteManager.Delete(note);
        //    }

        //    return base.Delete(category);
        //}

        public Category GetCategoryById(int id, bool withoutNote = true)
        {
            if (withoutNote == true)
            {
                //Burada isimsiz bir nesne oluşturulur ve bu nesnenin property'lerine veri tabanından istenilen sütün değerleri basılır. Daha sonra bu isimsiz nesne ile yeni bir Category nesnesi oluşturulur ve geriye döndürülür. Amaç: veritabanından Category nesnesine ait bütün sütün değerlerini çekmek yerine sadece istenilen sütünların değerlerini çekmek
                return QueryableList().Where(c => c.Id == id).Select(a => new
                {
                    _Id = a.Id,
                    _Title = a.Title,
                    _ModifiedUsername = a.ModifiedUsername,
                    _CreatedOn = a.CreatedOn,
                    _Description = a.Description,
                    _ModifiedOn = a.ModifiedOn
                }).ToList()
                .Select(x => new Category
                {
                    Id = x._Id,
                    Title = x._Title,
                    Description = x._Description,
                    CreatedOn = x._CreatedOn,
                    ModifiedOn = x._ModifiedOn,
                    ModifiedUsername = x._ModifiedUsername
                }).FirstOrDefault();
            }
            else
            {
                return Find(x => x.Id == id);
            }
        }

        public List<Category> GetCategories(bool withoutNote = true)
        {
            if (withoutNote == true)
            {
                return QueryableList().Select(a => new
                {
                    _Id = a.Id,
                    _Title = a.Title,
                    _Description = a.Description,
                    _ModifiedOn = a.ModifiedOn,
                    _CreatedOn = a.CreatedOn,
                    _ModifiedUsername = a.ModifiedUsername
                }).ToList()
                .Select(x => new Category
                {
                    Id = x._Id,
                    Title = x._Title,
                    Description = x._Description,
                    CreatedOn = x._CreatedOn,
                    ModifiedOn = x._ModifiedOn,
                    ModifiedUsername = x._ModifiedUsername
                }).ToList();
            }
            else
            {
                return List();
            }
        }
        public List<Category> GetCategories(bool withoutNote, int skipCount, int takeCount)
        {
            if (withoutNote == true)
            {
                return QueryableList().OrderByDescending(c => c.ModifiedOn).Skip(skipCount).Take(takeCount).Select(a => new
                {
                    _Id = a.Id,
                    _Title = a.Title,
                    _Description = a.Description,
                    _ModifiedOn = a.ModifiedOn,
                    _CreatedOn = a.CreatedOn,
                    _ModifiedUsername = a.ModifiedUsername
                }).ToList()
                .Select(x => new Category
                {
                    Id = x._Id,
                    Title = x._Title,
                    Description = x._Description,
                    CreatedOn = x._CreatedOn,
                    ModifiedOn = x._ModifiedOn,
                    ModifiedUsername = x._ModifiedUsername
                }).ToList();
            }
            else
            {
                return QueryableList().OrderByDescending(c => c.ModifiedOn).Skip(skipCount).Take(takeCount).ToList();
            }
        }
    }
}
