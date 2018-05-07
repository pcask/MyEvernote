using System;
using System.Collections.Generic;
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
        [Required, StringLength(60)]
        public string Title { get; set; }

        [Required, StringLength(2000)]
        public string Text { get; set; }

        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }

        // Normalde bu property olmasada aşağıdaki navigation property olan Category sayesinde bu notun Category nesnesine ulaşabilir, çoğu zaman bu nesne yerine sadece Id bilgisi lazım olacağını öngörüyorsak bunun gibi yardımcı property'ler oluşturabiliriz. Bu sayede Server' a fazladan sorgu atılmamasını ve ram'e yeni bir nesne çıkarmamış oluruz. Dezavantajı ise Database de oluşturulacak olan Note tablosuna yeni bir sütünun eklenmesi ve Database'in şişmesidir.
        public int CategoryId { get; set; }

        public virtual EvernoteUser Owner { get; set; }
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
