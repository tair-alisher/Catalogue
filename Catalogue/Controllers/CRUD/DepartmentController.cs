using System.Web.Mvc;
using PagedList;

using Catalogue.Util;
using Catalogue.Core;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Controllers.CRUD
{
    public class DepartmentController : Controller
    {
        IUnitOfWork unit;

        public DepartmentController()
        {
            this.unit = new UnitOfWork();
        }
        public DepartmentController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList departments = unit.Departments
                .GetDepartmentsWithAdministrationsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return PartialView(departments);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList departments = unit.Departments
                .GetDepartmentsWithAdministrationsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return View(departments);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Department administration = unit
                .Departments
                .GetSingleDepartmentWithAdministrationById((int)id);

            return View(administration);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            SelectList administrationList = new SelectList(
                unit.Administrations.GetAll(),
                "AdministrationId",
                "AdministrationName"
                );
            ViewBag.AdministrationList = administrationList;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Department collection)
        {
            try
            {
                unit.Departments.Create(collection);
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

            Department department = unit
                .Departments
                .Get((int)id);

            if (department == null)
                return HttpNotFound();

            SelectList administrationList = new SelectList(
                unit.Administrations.GetAll(),
                "AdministrationId",
                "AdministrationName"
                );
            ViewBag.AdministrationList = administrationList;

            return View(department);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Department collection)
        {
            try
            {
                unit.Departments.Update(collection);
                unit.Save();
                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            Department department = unit
                .Departments
                .Get((int)id);

            if (department != null)
                return PartialView("Delete", department);

            return View("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ActionName("Delete")]
        public ActionResult Delete(int? id, Department collection)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();

                unit.Departments.Delete((int)id);
                unit.Save();

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }
    }
}