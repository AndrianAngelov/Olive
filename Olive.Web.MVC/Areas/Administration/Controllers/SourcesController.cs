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
using System.Threading;

namespace Olive.Web.MVC.Areas.Administration.Controllers
{
    public class SourcesController : AdministrationCRUDController
    {
        private IUowData db = new UowData();

        //
        // GET: /Administration/Sources/

        public ViewResult Index()
        {

            //return View(db.Sources.All().ToList());

            SourcesIndexViewModel indexModel = new SourcesIndexViewModel();
            indexModel.AllSources = db.Sources.All();
            indexModel.NewSource = new Source();
            return View(indexModel);
        }

        //
        // POST: /Administration/Sources/Create
        [HttpPost]
        public ActionResult Create(Source newSource)
        {
            var indexModel = new SourcesIndexViewModel();
            var addSource = new Source();
            if (ModelState.IsValid)
            {
                //addSource.SourceID = newSource.SourceID;
                addSource.Name = newSource.Name;

                db.Sources.Add(addSource);

                db.SaveChanges();

            }

            return PartialView("_SourcePartial", addSource);
        }
        
        //
        // GET: /Administration/Sources/Edit/5
        public ActionResult Edit(int id)
        {
            Source source = db.Sources.All().Single(s => s.SourceID == id);
            return PartialView(source);
        }

        //
        // POST: /Administration/Sources/Edit/5
        [HttpPost]
        public ActionResult Edit(Source model,int id)
        {
            var indexModel = new SourcesIndexViewModel();
            Source source = db.Sources.GetById(id);
            if (ModelState.IsValid)
            {
                source.Name = model.Name;
                db.Sources.Update(source);
                db.SaveChanges();
            }
            //model.SourceID = id;
            //return PartialView("_SourcePartial", model);
            return Json(new { sourceID = id, sourceName = model.Name });
        }

        //
        // GET: /Administration/Sources/Delete/5
        public ActionResult Delete(int id)
        {
            Source source = db.Sources.All().Single(s => s.SourceID == id);
            //return View(source);
            return PartialView(source);
        }

        //
        // POST: /Administration/Sources/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Source source = db.Sources.All().Single(s => s.SourceID == id);
            db.Sources.Delete(source);
            db.SaveChanges();
            //return Json(new { id = id });
            return Content(id.ToString());
        }
    }
}