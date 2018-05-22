using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Common.Helpers;
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
    public class EvernoteUserManager : ManagerBase<EvernoteUser>
    {
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterVO data)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);

            if (user != null)
            {
                if (user.Username == data.Username)
                    blr.AddError(ErrorCode.UsernameAlreadyExists, "Bu kullanıcı adı kullanılıyor!");

                if (user.Email == data.Email)
                    blr.AddError(ErrorCode.EmailAlreadyExists, "Bu e-posta adresi kullanılıyor!");
            }
            else
            {
                int affectedRow = base.Insert(new EvernoteUser
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = Crypto.HashPassword(data.Password),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    ProfileImageFileName = "avatar.png"
                });

                if (affectedRow > 0)
                {
                    blr.Result = Find(x => x.Username == data.Username && x.Email == data.Email);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{blr.Result.ActivateGuid}";
                    string body = $"Merhaba {blr.Result.Username}, <br/><br/> Hesabınızı aktifleştirmek için lütfen <a href ='{activateUri}' target='_bank'>tıklayınız...</a>";

                    MailHelper.SendMail(body, blr.Result.Email, "MyEvernote Hesap Aktivasyonu");
                }
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();
            blr.Result = Find(x => x.ActivateGuid == activateId);

            if (blr.Result != null)
            {
                if (blr.Result.IsActive)
                {
                    blr.AddInfo(InfoCode.UserAlreadyActive, "Bu hesap zaten aktif");

                    return blr;
                }

                blr.Result.IsActive = true;
                Update(blr.Result);
            }
            else
            {
                blr.AddError(ErrorCode.ActivateIdDoesNotExists, "Aktifleştirilecek herhangi bir hesap bulunamadı!");
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginVO data)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();
            blr.Result = Find(x => x.Username == data.Username);

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
                    {
                        blr.AddError(ErrorCode.UserIsNotActive, "Hesap aktivasyonu yapılmamış!");
                        blr.AddInfo(InfoCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");

                        string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                        string activateUri = $"{siteUri}/Home/UserActivate/{blr.Result.ActivateGuid}";
                        string body = $"Merhaba {blr.Result.Username} <br/><br/> Hesabınızı aktifleştirmek için lütfen <a href ='{activateUri}' target='_bank'>tıklayınız...</a>";

                        MailHelper.SendMail(body, blr.Result.Email, "MyEvernote Hesap Aktivasyonu");
                    }
                }
            }
            else
            {
                blr.AddError(ErrorCode.UsernameOrPassWrong, "Kullanıcı adı veya şifre hatalı!");
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();
            blr.Result = Find(x => x.Id == id);

            if (blr.Result == null)
            {
                blr.AddError(ErrorCode.UserNotFound, "Kullanıcı bulunamadı!");
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> ChangePass(EvernoteUser currrentUser, ChangePasswordVO data)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();
            blr.Result = Find(x => x.Id == currrentUser.Id);

            if (blr.Result != null)
            {
                if (!Crypto.VerifyHashedPassword(blr.Result.Password, data.CuPassword))
                {
                    blr.AddError(ErrorCode.UserPassWrong, "Kullanıcı şifresi yanlış!");
                }
                else
                {
                    if (data.Password == data.RePassword)
                    {
                        blr.Result.Password = Crypto.HashPassword(data.Password);
                        Update(blr.Result);
                    }
                    else
                    {
                        blr.AddError(ErrorCode.UserPassAndRePassDontMatch, "Şifre ile Şifre (Tekrar) alanları uyuşmuyor");
                    }
                }
            }
            else
            {
                blr.AddError(ErrorCode.UserNotFound, "Kullanıcı bulunamadı!");
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => (x.Username == data.Username || x.Email == data.Email) && x.Id != data.Id);
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();

            if (db_user != null)
            {
                if (db_user.Username == data.Username)
                {
                    blr.AddError(ErrorCode.UsernameAlreadyExists, "Kullanıcı adı kullanılıyor");
                }

                if (db_user.Email == data.Email)
                {
                    blr.AddError(ErrorCode.EmailAlreadyExists, "E-posta adresi kullanılıyor");
                }

                return blr;
            }


            blr.Result = Find(x => x.Id == data.Id);

            blr.Result.Username = data.Username;
            blr.Result.Email = data.Email;
            blr.Result.Name = data.Name;
            blr.Result.Surname = data.Surname;

            if (!String.IsNullOrEmpty(data.ProfileImageFileName))
            {
                blr.Result.ProfileImageFileName = data.ProfileImageFileName;
            }


            if (base.Update(blr.Result) == 0)
            {
                blr.AddError(ErrorCode.ProfileCouldNotUpdated, "Profil güncellenemedi!");
            }

            return blr;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();

            blr.Result = Find(x => x.Id == id);

            if (blr.Result != null)
            {
                if (Delete(blr.Result) == 0)
                {
                    blr.AddError(ErrorCode.UserCouldNotRemove, "Kullanıcı silme işlemi başarısız!");
                    return blr;
                }
            }
            else
            {
                blr.AddError(ErrorCode.UserNotFound, "Kullanıcı bulunamadı");
            }

            return blr;
        }

        // Method Hiding
        public new BusinessLayerResult<EvernoteUser> Insert(EvernoteUser data)
        {
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();

            blr.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                    blr.AddError(ErrorCode.UsernameAlreadyExists, "Bu kullanıcı adı kullanılıyor!");

                if (user.Email == data.Email)
                    blr.AddError(ErrorCode.EmailAlreadyExists, "Bu e-posta adresi kullanılıyor!");
            }
            else
            {
                blr.Result.Password = Crypto.HashPassword(data.Password);
                blr.Result.ActivateGuid = Guid.NewGuid();
                blr.Result.ProfileImageFileName = "avatar.png";

                if (base.Insert(blr.Result) == 0)
                {
                    blr.AddError(ErrorCode.UserCouldNotInserted, "Kullanıcı ekleme işlemi başarısız!");
                }
            }

            return blr;
        }

        public new BusinessLayerResult<EvernoteUser> Update(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => (x.Username == data.Username || x.Email == data.Email) && x.Id != data.Id);
            BusinessLayerResult<EvernoteUser> blr = new BusinessLayerResult<EvernoteUser>();

            if (db_user != null)
            {
                if (db_user.Username == data.Username)
                {
                    blr.AddError(ErrorCode.UsernameAlreadyExists, "Kullanıcı adı kullanılıyor");
                }

                if (db_user.Email == data.Email)
                {
                    blr.AddError(ErrorCode.EmailAlreadyExists, "E-posta adresi kullanılıyor");
                }

                return blr;
            }

            blr.Result = Find(x => x.Id == data.Id);

            blr.Result.Username = data.Username;
            blr.Result.Email = data.Email;
            blr.Result.Name = data.Name;
            blr.Result.Surname = data.Surname;
            blr.Result.IsActive = data.IsActive;
            blr.Result.IsAdmin = data.IsAdmin;
            blr.Result.Password = Crypto.HashPassword(data.Password);

            if (base.Update(blr.Result) == 0)
            {
                blr.AddError(ErrorCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi!");
            }

            return blr;
        }
    }
}
