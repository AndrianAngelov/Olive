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
    public class UnitsController : Controller
    {
        private IUowData db = new UowData();

        //
        // GET: /Administration/Units/

        public ViewResult Index()
        {
            UnitsIndexViewModel indexModel = new UnitsIndexViewModel();
            indexModel.AllUnits = db.Units.All();
            indexModel.NewUnit = new Unit();
            return View(indexModel);
        }

        //
        // POST: /Administration/Units/Create
        [HttpPost]
        public ActionResult Create(Unit newUnit)
        {
            var indexModel = new UnitsIndexViewModel();
            var addUnit = new Unit();
            if (ModelState.IsValid)
            {
                //addSource.SourceID = newSource.SourceID;
                addUnit.UnitName = newUnit.UnitName;
                addUnit.UnitShortName = newUnit.UnitShortName;

                db.Units.Add(addUnit);
                db.SaveChanges();
            }

            return PartialView("_UnitPartial", addUnit);
        }
        
        //
        // GET: /Administration/Units/Edit/5
        public ActionResult Edit(int id)
        {
            Unit unit = db.Units.GetById(id);
            return PartialView(unit);
        }

        //
        // POST: /Administration/Units/Edit/5
        [HttpPost]
        public ActionResult Edit(Unit model, int id)
        {
            var indexModel = new UnitsIndexViewModel();
            Unit unit = db.Units.GetById(id);
            if (ModelState.IsValid)
            {
                unit.UnitName = model.UnitName;
                unit.UnitShortName = model.UnitShortName;
                db.Units.Update(unit);
                db.SaveChanges();
            }
            //model.SourceID = id;
            //return PartialView("_SourcePartial", model);
            return Json(new { unitID = id, unitName = model.UnitName, unitShortName = model.UnitShortName });
        }

        //
        // GET: /Administration/Units/Delete/5
        public ActionResult Delete(int id)
        {
            Unit unit = db.Units.GetById(id);
            return PartialView(unit);
        }

        //
        // POST: /Administration/Units/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit unit = db.Units.GetById(id);
            db.Units.Delete(unit);
            db.SaveChanges();
            //return Json(new { id = id });
            return Content(id.ToString());
        }
    }
}