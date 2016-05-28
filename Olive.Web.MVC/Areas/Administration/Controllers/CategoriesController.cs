using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Olive.Data;
using Olive.Data.Uow;
using Olive.Web.MVC.Areas.Administration.ViewModels;
using Olive.Web.MVC.Areas.Administration.Models;

namespace Olive.Web.MVC.Areas.Administration.Controllers
{ 
    public class CategoriesController : Controller
    {
        private IUowData db = new UowData();

        public CategoriesController() 
        {
            
        }
        //
        // GET: /Administration/Categories/

        public ViewResult Index()
        {
            //var categories = db.Categories.AllChildrenCategories().Include(c => c.Category1);
            //return View(categories.ToList());
            AdminCategoriesIndexViewModel categoriesVM = new AdminCategoriesIndexViewModel()
            {
                ParentCategories = this.db.Categories.AllParentCategories(),
                ChildrenCategories = this.db.Categories.AllChildrenCategories()
            };
            return View(categoriesVM);
        }

        #region Details/Create from scaffold 
        //public ViewResult Details(int id)
        //{
        //    Category category = db.Categories.GetById(id);
        //    return View(category);
        //}

        //public ActionResult Create()
        //{
        //    ViewBag.ParentCategoryID = new SelectList(db.Categories.AllParentCategories(), "CategoryID", "Name");
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Categories.Add(category);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ParentCategoryID = new SelectList(db.Categories.AllParentCategories(), "CategoryID", "Name", category.ParentCategoryID);
        //    return View(category);
        //} 
        #endregion

        [HttpPost]
        public ActionResult Create(AdminCategoriesIndexViewModel categoryiesModel)
        {
            Category newCategory = new Category();
            newCategory.Name = categoryiesModel.Name;
            if (categoryiesModel.NewCategoryType==CategoryType.Parent)
            {
                newCategory.ParentCategoryID = null;
            }
            else
            {
                newCategory.ParentCategoryID = categoryiesModel.SelectedParentCategoryID;
            }

            db.Categories.Add(newCategory);
            db.SaveChanges();

            categoryiesModel.ParentCategories = this.db.Categories.AllParentCategories();
            return PartialView("_CategoriesListPartial", categoryiesModel.ParentCategories);

            //ViewBag.ParentCategoryID = new SelectList(db.Categories.AllParentCategories(), "CategoryID", "Name", category.ParentCategoryID);
            //return View("Index",categoryiesModel);
        } 

        //
        // GET: /Administration/Categories/Edit/5
        //public ActionResult Edit(int id)
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.GetById(id);
            AdminCategoriesIndexViewModel categoryVM = new AdminCategoriesIndexViewModel();
            List<Category> parentsCategories = this.db.Categories.AllParentCategories().ToList();

            categoryVM.CategoryID=category.CategoryID;
            categoryVM.Name=category.Name;
            
            if (category.Category1 == null)
            {
                categoryVM.NewCategoryType = CategoryType.Parent;
                parentsCategories.Remove(category);
            }
            else
            {
                categoryVM.NewCategoryType = CategoryType.Child;
                categoryVM.SelectedParentCategoryID = category.Category1.CategoryID;
            }
            //categoryVM.ParentCategories = this.db.Categories.AllParentCategories();
            categoryVM.ParentCategories = parentsCategories;
            return PartialView("Edit",categoryVM);
        }

        //
        // POST: /Administration/Categories/Edit/5
        [HttpPost]
        public ActionResult Edit(AdminCategoriesIndexViewModel categoryVM)
        {
            Category category = db.Categories.GetById(categoryVM.CategoryID);
            if (ModelState.IsValid)
            {
                category.Name = categoryVM.Name;

                //children category become parent
                if (categoryVM.NewCategoryType == CategoryType.Parent && category.Category1!=null)
                {
                    category.ParentCategoryID = null;
                }
                if (categoryVM.NewCategoryType == CategoryType.Child && category.Category1 == null)
                {
                    category.ParentCategoryID = categoryVM.SelectedParentCategoryID;
                }
                //else
                //{
                //    newCategory.ParentCategoryID = categoryiesModel.SelectedParentCategoryID;
                //}

                db.Categories.Update(category);
                db.SaveChanges();
            }
            //model.SourceID = id;
            //return PartialView("_SourcePartial", model);
            //return Json(new { categoryID = categoryVM.CategoryID, categoryName = categoryVM.Name});
            categoryVM.ParentCategories = this.db.Categories.AllParentCategories();
            return PartialView("_CategoriesListPartial", categoryVM.ParentCategories);
        }

        //
        // GET: /Administration/Categories/Delete/5
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.GetById(id);
            return PartialView(category);
        }

        //
        // POST: /Administration/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Category category = db.Categories.GetById(id);
            if (category.ParentCategoryID==null)
            {
                foreach (var childcategory in category.Categories1.ToList())
                {
                    db.Categories.Delete(childcategory);
                }
            }
            db.Categories.Delete(category);
            db.SaveChanges();
            return Content(id.ToString());
        }

        //all categories as JSON
        public ActionResult GetAllParentCategories()
        {
            var parentCategories = db.Categories.AllParentCategories();
            var parentCategoriesJSON = parentCategories.Select(x => new
            {
                CategoryID = x.CategoryID,
                Name = x.Name
            });
            return Json(parentCategoriesJSON, JsonRequestBehavior.AllowGet);
        }
    }
}