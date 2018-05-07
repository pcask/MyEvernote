using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EF
{
    class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Sezar",
                Surname = "Yüce",
                Username = "pcask",
                Email = "sezer.ayran@gmail.com",
                Password = "123456",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "pcask"
            };

            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Sendemi",
                Surname = "Brutus",
                Username = "komplo123",
                Email = "sezer.ayran@gmail.com",
                Password = "654321",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "pcask"
            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            // Birazda uydurma kullanıcı üretelim
            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Username = $"user{i + 1}",
                    Email = FakeData.NetworkData.GetEmail(),
                    Password = "123",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now.AddYears(-1)),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i + 1}"
                };

                context.EvernoteUsers.Add(user);
            }

            context.SaveChanges();

            List<EvernoteUser> userList = context.EvernoteUsers.ToList();

            // Kategori Ekleme İşlemi...
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetCity(),
                    Description = FakeData.PlaceData.GetStreetName(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "pcask"
                };

                // Not Ekleme İşlemi...
                for (int k = 0; k < FakeData.NumberData.GetNumber(6, 8); k++)
                {
                    EvernoteUser noteOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.PlaceData.GetCountry(),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(2, 8)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = noteOwner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now.AddYears(-1)),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = noteOwner.Username
                    };

                    cat.Notes.Add(note);

                    // Yorum Ekleme İşlemi...
                    for (int j = 0; j < FakeData.NumberData.GetNumber(2, 5); j++)
                    {
                        EvernoteUser commentOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = commentOwner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now.AddYears(-1)),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = commentOwner.Username
                        };

                        note.Comments.Add(comment);
                    }


                    // Like Ekleme İşlemi...
                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[m]
                        };

                        note.Likes.Add(liked);
                    }
                }

                context.Categories.Add(cat);
            }

            context.SaveChanges();
        }
    }
}
