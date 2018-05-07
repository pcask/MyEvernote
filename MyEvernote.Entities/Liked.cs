using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    // Bu sınıf Note ile EvernoteUser ara Tablosudur. Bir not birden fazla kullanıcı tarafından Like'lanabilir. Aynı zamanda bir kullanıcı birden fazla Note Like'layabilir.
    [Table("Likes")]
    public class Liked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual Note Note { get; set; }
        public virtual EvernoteUser LikedUser { get; set; }
    }
}
