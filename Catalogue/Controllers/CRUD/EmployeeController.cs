﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;
using System.Net;
using System.Data.Entity;
using PagedList;
using System.IO;
using Catalogue.Models;
using System.Data.Entity.Validation;
using System.Web.Helpers;

namespace Catalogue.Controllers.CRUD
{
    public class EmployeeController : Controller
    {
        CatalogueContext db = new CatalogueContext();
        

        // Ajax pagination PartialView Employee 
        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(db.Employees.Include(e => e.Department).Include(p => p.Position).OrderBy(i => i.EmployeeFullName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Employee
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            List<Position> positions = db.Positions.ToList();
            ViewBag.Positions = positions;

            List<Department> departments = db.Departments.ToList();
            ViewBag.Departments = departments;

            List<Administration> admins = db.Administrations.ToList();
            ViewBag.Admins = admins;

            List<Division> divisions = db.Divisions.ToList();
            ViewBag.Divisions = divisions;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.Employees.Include(e => e.Department).Include(p => p.Position).OrderBy(i => i.EmployeeFullName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Employee/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {

            if (id == null)
                return HttpNotFound();
            
            Employee employee = db.Employees.Include(p => p.Position).Include(d => d.Department).SingleOrDefault(e => e.EmployeeId == id);
            return View(employee);
        }

        [HttpGet]
        // GET: Employee/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            SelectList departmentList = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            ViewBag.DepartmentList = departmentList;
            SelectList positionList = new SelectList(db.Positions, "PositionId", "PositionName");
            ViewBag.PositionList = positionList;
            return View();
        }
        
        // POST: Employee/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Employee collection, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {          
                if (productImg == null)
                {
                    collection.EmployeePhoto = "default-avatar.png";
                }
                else
                {
                    var fileName = Path.GetFileName(productImg.FileName);
                    
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;

                    var directoryToSave = Server.MapPath(Url.Content("~/images"));

                    var pathToSave = Path.Combine(directoryToSave, fileName);

                    productImg.SaveAs(pathToSave);
                    collection.EmployeePhoto = fileName;
                }
            }
            db.Employees.Add(collection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Employee employee = db.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();

            SelectList departmentList = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            ViewBag.DepartmentList = departmentList;
            SelectList positionList = new SelectList(db.Positions, "PositionId", "PositionName");
            ViewBag.PositionList = positionList;
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Employee collection, HttpPostedFileBase productImg, string photo)
        {
            if (ModelState.IsValid)
            {
                if (productImg == null)
                {
                    collection.EmployeePhoto = photo;
                }
                else if (productImg != null)
                {
                    string fullPath = Request.MapPath("~/images/" + photo);
                    if (System.IO.File.Exists(fullPath) && photo != "default-avatar.png")
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    var fileName = Path.GetFileName(productImg.FileName);

                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;

                    var directoryToSave = Server.MapPath(Url.Content("~/images"));

                    var pathToSave = Path.Combine(directoryToSave, fileName);
                    productImg.SaveAs(pathToSave);
                    collection.EmployeePhoto = fileName;
                }
            }
            db.Entry(collection).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Employee/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Employee employee = db.Employees.Include(p => p.Position).Include(d => d.Department).SingleOrDefault(e => e.EmployeeId == id);
            if (employee != null)
                return PartialView("Delete", employee);
            return View("Index");
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id, string photoName)
        {
            Employee employee = new Employee();
            try
            {
                if (id == null)
                    return HttpNotFound();
                employee = db.Employees.Find(id);
                if (employee == null)
                    return HttpNotFound();

                string fullPath = Request.MapPath("~/images/" + photoName);
                if (System.IO.File.Exists(fullPath) && photoName != "default-avatar.png")
                {
                    System.IO.File.Delete(fullPath);
                }

                db.Employees.Remove(employee);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}