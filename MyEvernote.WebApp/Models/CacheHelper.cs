using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEvernote.WebApp.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("cacheCategory");

            if (result == null)
            {
                CategoryManager categoryManager = new CategoryManager();

                result = categoryManager.GetCategories(withoutNote: true);

                WebCache.Set("cacheCategory", result, 20, true);
            }

            return result;
        }

        public static void RemoveCategoryFromCache()
        {
            Remove("cacheCategory");
        }

        private static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}