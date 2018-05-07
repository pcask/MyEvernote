using MyEvernote.DataAccessLayer.EF;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager
    {
        private Repository<Category> catRepo = new Repository<Category>();

        public Category GetCategoryById(int id, bool withoutNote)
        {
            if (withoutNote == true)
            {
                return catRepo.QueryableList().Where(c => c.Id == id).Select(a => new
                {
                    _Id = a.Id,
                    _Title = a.Title,
                    _ModifiedUsername = a.ModifiedUsername,
                    _CreatedOn = a.CreatedOn,
                    _Description = a.Description,
                    _ModifiedOn = a.ModifiedOn
                }).ToList()
                .Select(x => new Category
                {
                    Id = x._Id,
                    Title = x._Title,
                    Description = x._Description,
                    CreatedOn = x._CreatedOn,
                    ModifiedOn = x._ModifiedOn,
                    ModifiedUsername = x._ModifiedUsername
                }).FirstOrDefault();
            }
            else
            {
                return catRepo.Find(x => x.Id == id);
            }
        }

        //public Category GetCategoryById(int id,int skipNoteCount, int takeNoteCount)
        //{
        //    return catRepo.QueryableList().
        //}

        public List<Category> GetCategories(bool withoutNote)
        {
            if (withoutNote == true)
            {
                return catRepo.QueryableList().Select(a => new
                {
                    _Id = a.Id,
                    _Title = a.Title,
                    _Description = a.Description,
                    _ModifiedOn = a.ModifiedOn,
                    _CreatedOn = a.CreatedOn,
                    _ModifiedUsername = a.ModifiedUsername
                }).ToList()
                .Select(x => new Category
                {
                    Id = x._Id,
                    Title = x._Title,
                    Description = x._Description,
                    CreatedOn = x._CreatedOn,
                    ModifiedOn = x._ModifiedOn,
                    ModifiedUsername = x._ModifiedUsername
                }).ToList();
            }
            else
            {
                return catRepo.List();
            }
        }
        public List<Category> GetCategories(bool withoutNote, int skipCount, int takeCount)
        {
            if (withoutNote == true)
            {
                return catRepo.QueryableList().OrderByDescending(c => c.ModifiedOn).Skip(skipCount).Take(takeCount).Select(a => new
                {
                    _Id = a.Id,
                    _Title = a.Title,
                    _Description = a.Description,
                    _ModifiedOn = a.ModifiedOn,
                    _CreatedOn = a.CreatedOn,
                    _ModifiedUsername = a.ModifiedUsername
                }).ToList()
                .Select(x => new Category
                {
                    Id = x._Id,
                    Title = x._Title,
                    Description = x._Description,
                    CreatedOn = x._CreatedOn,
                    ModifiedOn = x._ModifiedOn,
                    ModifiedUsername = x._ModifiedUsername
                }).ToList();
            }
            else
            {
                return catRepo.QueryableList().OrderByDescending(c => c.ModifiedOn).Skip(skipCount).Take(takeCount).ToList();
            }
        }
    }
}
