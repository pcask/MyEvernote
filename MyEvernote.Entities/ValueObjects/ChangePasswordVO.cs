using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.ValueObjects
{
    public class ChangePasswordVO
    {
        [DisplayName("Şifreniz"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(25, MinimumLength = 8, ErrorMessage = "{0} alanı en az {2} en fazla {1} karakter olmalı!"),
            DataType(DataType.Password),
            RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{6,}$", ErrorMessage = "{0} alanı en az bir tane büyük, küçük harf, rakam ve özel karakter içermelidir!")]
        public string CuPassword { get; set; }

        [DisplayName("Yeni Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(25, MinimumLength = 8, ErrorMessage = "{0} alanı en az {2} en fazla {1} karakter olmalı!"),
            DataType(DataType.Password),
            RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{6,}$", ErrorMessage = "{0} alanı en az bir tane büyük, küçük harf, rakam ve özel karakter içermelidir!")]
        public string Password { get; set; }

        [DisplayName("Yeni Şifre (Tekrar)"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(25, MinimumLength = 8, ErrorMessage = "{0} alanı en az {2} en fazla {1} karakter olmalı!"),
            DataType(DataType.Password),
            Compare(nameof(Password), ErrorMessage = "{0} ile {1} alanları uyuşmamaktadır!")]
        public string RePassword { get; set; }
    }
}
