using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ValueObjects
{
    public class RegisterVO
    {
        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalı!"),
            EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalı!")]
        public string Username { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(25, MinimumLength = 8, ErrorMessage = "{0} alanı en az {2} en fazla {1} karakter olmalı!"),
            DataType(DataType.Password),
            RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{6,}$", ErrorMessage = "{0} alanı en az bir tane büyük, küçük harf, rakam ve özel karakter içermelidir!")]
        public string Password { get; set; }

        [DisplayName("Şifre (Tekrar)"),
            Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(25, MinimumLength = 8, ErrorMessage = "{0} alanı en az {2} en fazla {1} karakter olmalı!"),
            DataType(DataType.Password),
            Compare(nameof(Password), ErrorMessage = "{0} ile {1} alanları uyuşmamaktadır!")]
        public string RePassword { get; set; }

        [DisplayName("Hizmet Sözleşmesi")]
        public bool ContractAccepted { get; set; }
    }
}