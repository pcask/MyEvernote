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
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("Ad"), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Name { get; set; }

        [DisplayName("Soyad"), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez!"), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Username { get; set; }

        [DisplayName("E-mail"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez!"), 
            StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Email { get; set; }

        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez!"), 
            StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")] // Password'ü Database'e şifrelenmiş bir şekilde kaydetmeliyiz o yüzden max. karakter 100 
        public string Password { get; set; }

        [StringLength(40)]
        public string ProfileImageFileName { get; set; }

        [Required, ScaffoldColumn(false)] // ScaffoldColumn(false) ile bu alan iskele malzemesi değildir demiş oluyoruz. EditorForModel'de gizlenecektir.
        public Guid ActivateGuid { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
