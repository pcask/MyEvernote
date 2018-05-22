using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }


        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // Her Notun birden fazla Yorumu olabilir.
            // Her Yorumun kesinlikle bir Notu olmalıdır.
            // Bu iki tablo arasındaki ilişkinin Silme kuralı Cascade olarak atanır. Yani herhangi bir Not silinirse o Nota ait Bütün Yorumlar silinecektir.
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Comments)
                .WithRequired(c => c.Note)
                .WillCascadeOnDelete(true);

            // Aynı şekilde herhangi bir Not silindiğinde o nota ait bütün Beğeniler silinecektir.
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes)
                .WithRequired(li => li.Note)
                .WillCascadeOnDelete(true);
        }
    }
}
