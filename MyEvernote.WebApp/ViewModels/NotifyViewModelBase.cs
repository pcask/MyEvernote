using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeout { get; set; }
        public string RedirectingPageName { get; set; }

        public NotifyViewModelBase()
        {
            IsRedirecting = true;
            RedirectingPageName = "Giriş";
            RedirectingUrl = "/Home/Login";
            RedirectingTimeout = 1000;
            Items = new List<T>();
        }

    }
}