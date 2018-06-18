using System.Web.Mvc;
using PagedList;

using Catalogue.Util;
using Catalogue.Core;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Controllers.CRUD
{
    public class AdministrationController : Controller
    {
        IUnitOfWork unit;
        
        public AdministrationController()
        {
            this.unit = new UnitOfWork();
        }
        public AdministrationController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList<Administration> administrations = unit
                .Administrations
                .GetAdministrationsWithDivisionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return PartialView(administrations);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList<Administration> administrations = unit
                .Administrations
                .GetAdministrationsWithDivisionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return View(administrations);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Administration administration = unit
                .Administrations
                .GetSingleAdministrationWithDivisionById((int)id);

            return View(administration);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            SelectList divisionList = new SelectList(
                unit.Divisions.GetAll(),
                "DivisionId",
                "DivisionName"
                );

            ViewBag.AdministrationList = divisionList;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Administration collection)
        {
            try
            {
                unit.Administrations.Create(collection);
                unit.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Administration administration = unit
                .Administrations
                .Get((int)id);

            if (administration == null)
                return HttpNotFound();

            SelectList divisionList = new SelectList(
                unit.Divisions.GetAll(),
                "DivisionId",
                "DivisionName"
                );

            ViewBag.AdministrationList = divisionList;
            return View(administration);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Administration collection)
        {
            try
            {
                unit.Administrations.Update(collection);
                unit.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            Administration administration = unit
                .Administrations
                .Get((int)id);

            if (administration != null)
                return PartialView("Delete", administration);

            return View("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ActionName("Delete")]
        public ActionResult Delete(int? id, Administration collection)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();

                unit.Administrations.Delete((int)id);
                unit.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}