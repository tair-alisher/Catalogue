using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

using Catalogue.Util;
using Catalogue.Core;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Controllers.CRUD
{
    public class EmployeeController : Controller
    {
        IUnitOfWork unit;

        public EmployeeController()
        {
            this.unit = new UnitOfWork();
        }
        public EmployeeController(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        
        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList<Employee> employees = unit
                .Employees
                .GetEmployeesWithDepartmentsAndPositionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return PartialView(employees);
        }

        // GET: Employee
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            List<Position> positions = unit
                .Positions
                .GetAll()
                .ToList();
            
            List<Department> departments = unit
                .Departments
                .GetAll()
                .ToList();
            
            List<Administration> admins = unit
                .Administrations
                .GetAll()
                .ToList();
            
            List<Division> divisions = unit
                .Divisions
                .GetAll()
                .ToList();

            ViewBag.Positions = positions;
            ViewBag.Departments = departments;
            ViewBag.Admins = admins;
            ViewBag.Divisions = divisions;

            int pageNumber = (page ?? 1);
            IPagedList<Employee> employees = unit
                .Employees
                .GetEmployeesWithDepartmentsAndPositionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return View(employees);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {

            if (id == null)
                return HttpNotFound();

            Employee employee = unit
                .Employees
                .GetSingleEmployeeWithDepartmentAndPositionById((int)id);

            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            SelectList departmentList = new SelectList(
                unit.Departments.GetAll(),
                "DepartmentId",
                "DepartmentName"
                );

            SelectList positionList = new SelectList(
                unit.Positions.GetAll(),
                "PositionId",
                "PositionName"
                );

            ViewBag.DepartmentList = departmentList;
            ViewBag.PositionList = positionList;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Employee collection, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {
                string imageDirectory = Server.MapPath(Url.Content("~/images"));
                unit
                    .Employees
                    .Create(collection, productImg, imageDirectory);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            SelectList departmentList = new SelectList(
                unit.Departments.GetAll(),
               "DepartmentId",
               "DepartmentName"
               );

            SelectList positionList = new SelectList(
                unit.Positions.GetAll(),
                "PositionId",
                "PositionName"
                );

            ViewBag.DepartmentList = departmentList;
            ViewBag.PositionList = positionList;

            Employee employee = unit.Employees.Get((int)id);
            if (employee == null)
                return HttpNotFound();

            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Employee collection, HttpPostedFileBase productImg, string photo)
        {
            if (ModelState.IsValid)
            {
                string imageDirectory = Server
                    .MapPath(Url.Content("~/images"));
                string oldImagePath = Request
                    .MapPath("~/images/" + photo);

                unit
                    .Employees
                    .Edit(
                        collection,
                        productImg,
                        photo,
                        imageDirectory,
                        oldImagePath
                    );
            }

            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Employee employee = unit
                .Employees
                .GetSingleEmployeeWithDepartmentAndPositionById((int)id);
            if (employee != null)
                return PartialView("Delete", employee);

            return View("Index");
        }
        
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id, string photoName)
        {
            Employee employee = new Employee();
            try
            {
                string imagePath = Request.MapPath("~/images/" + photoName);

                unit
                    .Employees
                    .Delete((int)id, imagePath);

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }
    }
}