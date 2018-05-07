using MyEvernote.DataAccessLayer.EF;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> userRepo = new Repository<EvernoteUser>();

        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterVO data)
        {
            EvernoteUser user = userRepo.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();


            if (user != null)
            {
                if (user.Username == data.Username)
                    blr.AddError(ErrorCode.UsernameAlreadyExists, "Bu kullanıcı adı kullanılıyor!");

                if (user.Email == data.Email)
                    blr.AddError(ErrorCode.EmailAlreadyExists, "Bu e-posta adresi kullanılıyor!");
            }
            else
            {
                int affectedRow = userRepo.Insert(new EvernoteUser
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = Crypto.HashPassword(data.Password),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (affectedRow > 0)
                {
                    blr.Result = userRepo.Find(x => x.Username == data.Username && x.Email == data.Email);

                    // Mail işlemleri
                }
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginVO data)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();

            blr.Result = userRepo.Find(x => x.Username == data.Username);

            if (blr.Result != null)
            {
                bool verifyPass = Crypto.VerifyHashedPassword(blr.Result.Password, data.Password);

                if (!verifyPass)
                {
                    blr.AddError(ErrorCode.UsernameOrPassWrong, "Kullanıcı adı veya şifre hatalı!");
                }
                else
                {
                    if (!blr.Result.IsActive)
                        blr.AddError(ErrorCode.UserIsNotActive, "Hesap aktivasyonu yapılmamış!");
                }
            }
            else
            {
                blr.AddError(ErrorCode.UsernameOrPassWrong, "Kullanıcı adı veya şifre hatalı!");
            }

            return blr;
        }
    }
}
