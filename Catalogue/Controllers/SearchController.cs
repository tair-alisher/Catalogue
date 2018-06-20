using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;

using Catalogue.Util;
using Catalogue.Core;
using Catalogue.Services;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Controllers
{
    public class SearchController : Controller
    {
        IUnitOfWork unit;
        ISearchEngine SearchEngine;

        public SearchController()
        {
            this.unit = new UnitOfWork();
            SearchEngine = new SearchEngine(unit);
        }
        public SearchController(IUnitOfWork unit, ISearchEngine SearchEngine = null)
        {
            this.unit = unit;
            if (SearchEngine == null)
                this.SearchEngine = new SearchEngine(this.unit);
            else
                this.SearchEngine = SearchEngine;
        }

        [HttpPost]
        public ActionResult EmployeeFilter(string name, int? page, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            name = name.Trim();

            IQueryable<Employee> employees = SearchEngine.EmployeeSearch(name, positionId, departmentId, administrationId, divisionId);

            string view = "";
            if (User.IsInRole("admin"))
                view = "~/Views/Search/AdminEmployeeFilter.cshtml";
            else if (User.IsInRole("manager"))
                view = "~/Views/Search/ManagerEmployeeFilter.cshtml";
            else
                view = "~/Views/Search/EmployeeFilter.cshtml";

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return PartialView(view, employees.ToPagedList(pageNumber, pageSize));
        }

        // Forms not found partial view
        public ActionResult NotFoundResult()
        {
            return PartialView("~/Views/Error/NotFoundError.cshtml");
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdministrationSearch(string title)
        {
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            string[] nameParts = title.Split(' ');
            ViewBag.Items = SearchEngine.AdministrationSearch(nameParts);

            return View("Administrations.chtml");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DepartmentSearch(string title)
        {
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            string[] nameParts = title.Split(' ');
            ViewBag.Items = SearchEngine.DepartmentSearch(nameParts);

            return View("Departments.cshtml");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DivsionSearch(string title)
        {
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            string[] nameParts = title.Split(' ');
            ViewBag.Items = SearchEngine.DivisionSearch(nameParts);

            return View("Divisions.cshtml");
        }

        [Authorize(Roles = "admin")]
        public ActionResult PositionSearch(string title)
        {
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            string[] nameParts = title.Split(' ');
            ViewBag.Items = SearchEngine.PositionSearch(nameParts);

            return View("Positions.cshtml");
        }

        [Authorize(Roles = "manager")]
        public ActionResult EmployeeDetails (int? id)
        {
            if (id == null)
                return HttpNotFound();

            Employee employee = unit.Employees
                .GetEmployeeWithRelationsById((int)id);

            return View(employee);
        }
    }
}