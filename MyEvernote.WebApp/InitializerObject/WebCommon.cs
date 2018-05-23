using MyEvernote.Common;
using MyEvernote.Entities;
using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.InitializerObject
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            EvernoteUser currentUser = CurrentSession.User;

            if (currentUser != null)
                return currentUser.Username;
            else
                return "system";
        }
    }
}