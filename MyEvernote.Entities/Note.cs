using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Notes")]
    public class Note : MyEntityBase
    {
        [DisplayName("Başlık"), Required, StringLength(60)]
        public string Title { get; set; }

        [DisplayName("İçerik"), Required, StringLength(2000)]
        public string Text { get; set; }

        [DisplayName("Taslak Mı ?")]
        public bool IsDraft { get; set; }

        [DisplayName("Beğenilme")]
        public int LikeCount { get; set; }

        // Normalde bu property olmasada aşağıdaki navigation property olan Category sayesinde bu notun Category nesnesine ulaşabilir, çoğu zaman bu nesne yerine sadece Id bilgisi lazım olacağını öngörüyorsak bunun gibi yardımcı property'ler oluşturabiliriz. Bu sayede Server' a fazladan sorgu atılmamasını ve ram'e yeni bir nesne çıkarmamış oluruz. Dezavantajı ise Database de oluşturulacak olan Note tablosuna yeni bir sütünun eklenmesi ve Database'in şişmesidir.
        [Required]
        public int CategoryId { get; set; }

        public virtual EvernoteUser Owner { get; set; }

        [DisplayName("Kategori")]
        public virtual Category Category { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }


        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}
