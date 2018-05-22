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
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Oluşturulma"), ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; } // DateTime tipi varsayılan olarak zaten boş geçilemez

        [DisplayName("Son Güncelleme"), ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Güncelleyen"), Required, StringLength(30), ScaffoldColumn(false)]
        public string ModifiedUsername { get; set; }
        // String tipinden ModifiedUsername yerine EvernoteUser tipinden bir user'da tutulabilirdi burada, fakat öyle bir senaryoda user silinmek istendiğinde ilişkili olduğu category'de silinmeli veya bu property'e null atanmalıdır, bu durumda da geçmişe dönük bilgiler yok olacaktır.
    }
}
