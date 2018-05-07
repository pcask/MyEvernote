using MyEvernote.Common;
using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.Abstract;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EF
{
    // Repository Pattern Kullanımı
    public class Repository<T> : RepositoryBase, IRepository<T> where T : class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>();
        }

        // Nesne listesi döndürmek için;
        public List<T> List()
        {
            return _objectSet.ToList();
        }

        // Koşullu nesne listesi döndürmek için;
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        // Tek bir nesne döndürmek için;
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        // List döndürdüğümüz methodlarda Linq ile oluşturulan sorgular veritabanına gider ve sonuç kümesini döner. IQueryable döndürdüğümüzde ise sonuç kümesinin sorgusu döner. Sonrasında bu sorguya linq ile yeni sorgular eklenebilir ve ne zaman veriyi çekmek istersek ( toList(), firstOrDefault() gibi...) sorgu o zaman veritabanına gidecektir.
        public IQueryable<T> QueryableList()
        {
            return _objectSet.AsQueryable<T>();
        }

        // Nesne eklemek için
        public int Insert(T obj)
        {
            if (obj is MyEntityBase)
            {
                DateTime now = DateTime.Now;

                MyEntityBase o = obj as MyEntityBase;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.common.GetCurrentUsername();
            }
            _objectSet.Add(obj);
            return Save();
        }

        // Update işlemi için önce update'edilecek nesne ele alınır, sonrasında nesnenin property'lerine istenilen değerler atanır ve ilgili DbSet'in SaveChanges() methodu çağrılır. Generic tipin propertlerini bul ve gelen parametreleri bu property'lere bas, sonra kaydet gibi bir update methodu yazılmaz.
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.common.GetCurrentUsername();
            }

            return Save();
        }

        // Nesne silme işlemi
        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        // Değişiklikleri kaydetme işlemi
        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
