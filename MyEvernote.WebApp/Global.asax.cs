using MyEvernote.Common;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.InitializerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyEvernote.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Yazmış olduğumuz MyException isimli Filter'ı Action seviyesinde, Controller seviyesinde veya buradaki gibi Application seviyesinde uygulayabiliriz.
            GlobalFilters.Filters.Add(new MyException());

            App.common = new WebCommon();
        }
    }
}
