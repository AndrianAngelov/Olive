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

namespace Olive.Web.MVC.Areas.Administration.Controllers
{
    public class IngredientsController : AdministrationCRUDController
    {
        private IUowData db = new UowData();

        //
        // GET: /Administration/Ingredients/

        public ViewResult Index()
        {
            IngredientsIndexViewModel indexModel = new IngredientsIndexViewModel();
            indexModel.AllIngredients = db.Ingredients.All();
            indexModel.NewIngredient= new Ingredient();
            return View(indexModel);
        }

        //
        // POST: /Administration/Ingredients/Create
        [HttpPost]
        public ActionResult Create(Ingredient newIngredient)
        {
            var indexModel = new IngredientsIndexViewModel();
            var addIngredient = new Ingredient();
            if (ModelState.IsValid)
            {
                //addSource.SourceID = newSource.SourceID;
                addIngredient.Name = newIngredient.Name;

                db.Ingredients.Add(addIngredient);

                db.SaveChanges();
            }

            return PartialView("_IngredientPartial", addIngredient);
        }
        
        //
        // GET: /Administration/Ingredients/Edit/5
 
        public ActionResult Edit(int id)
        {
            Ingredient ingredient = db.Ingredients.GetById(id);
            return PartialView(ingredient);
        }

        //
        // POST: /Administration/Ingredients/Edit/5

        [HttpPost]
        public ActionResult Edit(Ingredient model, int id)
        {
            var indexModel = new IngredientsIndexViewModel();
            Ingredient ingredient = db.Ingredients.GetById(id);
            if (ModelState.IsValid)
            {
                ingredient.Name = model.Name;
                db.Ingredients.Update(ingredient);
                db.SaveChanges();
            }
            //model.SourceID = id;
            //return PartialView("_SourcePartial", model);
            return Json(new { ingredientID = id, ingredientName = model.Name });
        }

        //
        // GET: /Administration/Ingredients/Delete/5
        public ActionResult Delete(int id)
        {
            Ingredient ingredient = db.Ingredients.GetById(id);
            return PartialView(ingredient);
        }

        //
        // POST: /Administration/Ingredients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = db.Ingredients.GetById(id);
            db.Ingredients.Delete(ingredient);
            db.SaveChanges();
            //return Json(new { id = id });
            return Content(id.ToString());
        }
    }
}