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
        public ActionResult DepartmentSearch(string title)
        {
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            string[] nameParts = title.Split(' ');
            ViewBag.Items = SearchEngine.DepartmentSearch(nameParts);

            return View("Departments.cshtml");
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

        // Forms a partial view with a list of found entities
        [Authorize(Roles = "admin")]
        public ActionResult AdminSearch(string title, string type)
        {
            string view = "~/Views/Search/";
            string[] words = title.ToLower().Split(' ');

            // returns not found if input string is empty
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

            if (type == "department")
            {
                List<Department> departments = BuildDepartmentSearchQuery(words).ToList();
                BindSearchResults(departments, ref view, "Departments.cshtml");
            }
            else if (type == "administration")
            {
                List<Administration> administrations = BuildAdministartionSearchQuery(words).ToList();
                BindSearchResults(administrations, ref view, "Administrations.cshtml");
            }
            else if (type == "position")
            {
                List<Position> positions = BuildPositionSearchQuery(words).ToList();
                BindSearchResults(positions, ref view, "Positions.cshtml");
            }
            else if (type == "division")
            {
                List<Division> divisions = BuildDivisionSearchQuery(words).ToList();
                BindSearchResults(divisions, ref view, "Divisions.cshtml");
            }

            return PartialView(view);
        }

        [Authorize(Roles = "manager")]
        public ActionResult EmployeeDetails (int? id)
        {
            if (id == null)
                return HttpNotFound();


            Employee employee = db.Employees.Include(p => p.Position).Include(d => d.Department).Include(e => e.Department.Administration).SingleOrDefault(e => e.EmployeeId == id);

            return View(employee);
        }


        // Binds entity search results and entity view
        private void BindSearchResults<T> (List<T> items, ref string view, string entityView)
        {
            if (items.Count <= 0)
            {
                view += "NotFound.cshtml";
            }
            else
            {
                ViewBag.Items = items;
                view += entityView;
            }
        }

        // Builds a search query that matches all words of the array 'words'
        private IEnumerable<Department> BuildDepartmentSearchQuery (params string[] words)
        {
            IEnumerable<Department> query = db.Departments
                .Include(a => a.Administration)
                .ToList()
                .Where(d => words.All(d.DepartmentName.ToLower().Contains));

            return query;
        }
        private IEnumerable<Administration> BuildAdministartionSearchQuery (params string[] words)
        {
            IEnumerable<Administration> query = db.Administrations
                .Include(a => a.Division)
                .ToList()
                .Where(d => words.All(d.AdministrationName.ToLower().Contains));

            return query;
        }
        private IEnumerable<Position> BuildPositionSearchQuery (params string[] words)
        {
            IEnumerable<Position> query = db.Positions
                .ToList()
                .Where(d => words.All(d.PositionName.ToLower().Contains));

            return query;
        }
        private IEnumerable<Division> BuildDivisionSearchQuery (params string[] words)
        {
            IEnumerable<Division> query = db.Divisions
                .ToList()
                .Where(d => words.All(d.DivisionName.ToLower().Contains));

            return query;
        }
    }
}