using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EF
{
    // An entity object cannot be referenced by multiple instances of IEntityChangeTracker hatasına karşılık olarak;
    // Singleton Pattern yaklaşımı ile DatabaseContext nesnemizin sadece bir kere örneklenmesini sağlıyoruz.
    public class RepositoryBase
    {
        protected static DatabaseContext context;

        // Object tipinden uydurma bir nesne üretiyoruz. Bunun sebebi deyiminin referans alacağı bir nesne istemesi
        private static object _lockSync = new object();

        // Bu sınıfı miras almayan bir sınıf tarafından instance oluşturulamasın diye constructor protected
        protected RepositoryBase()
        {
            CreateDbContext();
        }

        // Singleton Pattern yaklaşımı için sabit bir yardımcı method;
        private static void CreateDbContext()
        {
            if (context == null)
            {
                // Lock deyimi ile kitleme yaparak aynı anda iki thread'in çalıştırılması engellenir. Buradaki lock kullanımı ile Multiple thread işlemlerinde birden fazla iş parçacığının aynı anda DatabaseContext nesnemizi örneklemesinin önüne geçmiş oluruz.
                lock (_lockSync)
                {
                    if (context == null)
                        context = new DatabaseContext();
                }
            }
        }
    }
}
