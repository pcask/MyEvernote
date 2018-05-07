using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } // DateTime tipi varsayılan olarak zaten boş geçilemez
        public DateTime ModifiedOn { get; set; }

        [Required, StringLength(30)]
        public string ModifiedUsername { get; set; }
        // String tipinden ModifiedUsername yerine EvernoteUser tipinden bir user'da tutulabilirdi burada, fakat öyle bir senaryoda user silinmek istendiğinde ilişkili olduğu category'de silinmeli veya bu property'e null atanmalıdır, bu durumda da geçmişe dönük bilgiler yok olacaktır.
    }
}
