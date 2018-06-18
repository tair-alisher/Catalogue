using System.Web.Mvc;
using PagedList;

using Catalogue.Util;
using Catalogue.Core;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Controllers.CRUD
{
    public class DivisionController : Controller
    {
        IUnitOfWork unit;

        public DivisionController()
        {
            this.unit = new UnitOfWork();
        }
        public DivisionController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList<Division> divisions = unit
                .Divisions
                .GetDivisionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return PartialView(divisions);
        }
        
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList<Division> divisions = unit
                .Divisions
                .GetDivisionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return View(divisions);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Division division = unit.Divisions.Get((int)id);
            if (division == null)
                return HttpNotFound();

            return View(division);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Division collection)
        {
            try
            {
                unit.Divisions.Create(collection);
                unit.Save();
                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Division division = unit.Divisions.Get((int)id);
            if (division == null)
                return HttpNotFound();

            return View(division);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Division collection)
        {
            try
            {
                unit.Divisions.Update(collection);
                unit.Save();

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }
        
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Division division = unit.Divisions.Get((int)id);
            if (division != null)
                return PartialView("Delete", division);

            return View("Index");
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ActionName("Delete")]
        public ActionResult Delete(int? id, Division collection)
        {
            try
            {
                unit.Divisions.Delete((int)id);
                unit.Save();

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }
    }
}
