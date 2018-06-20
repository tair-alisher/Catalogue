using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IContext db;

        private const string DefaultPhotoName = "default-avatar.png";

        public EmployeeRepository(IContext context)
        {
            this.db = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return db.Employees;
        }

        public void Create(Employee employee, HttpPostedFileBase image, string imageDirectory)
        {
            if (image == null) { employee.EmployeePhoto = DefaultPhotoName; }
            else
            {
                string fileName = Path.GetFileName(image.FileName);
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;

                string filePath = Path.Combine(imageDirectory, fileName);
                image.SaveAs(filePath);

                employee.EmployeePhoto = fileName;
            }

            db.Employees.Add(employee);
            db.Save();
        }

        public void Edit(
            Employee employee,
            HttpPostedFileBase newImage,
            string oldImage,
            string imageDirectory,
            string oldImagePath)
        {
            if (newImage == null) { employee.EmployeePhoto = oldImage; }
            else
            {
                if (File.Exists(oldImagePath) && oldImage != DefaultPhotoName)
                    File.Delete(oldImagePath);

                string fileName = Path.GetFileName(newImage.FileName);
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;

                string filePath = Path.Combine(imageDirectory, fileName);
                newImage.SaveAs(filePath);

                employee.EmployeePhoto = fileName;
            }

            db.SetModified(employee);
            db.Save();
        }

        public Employee Get(int id)
        {
            return db.Employees.Find(id);
        }

        public void Update(Employee employee)
        {
            db.SetModified(employee);
        }

        public void Delete(int id, string imagePath)
        {
            if (File.Exists(imagePath) && imagePath.Split('/').Last() != DefaultPhotoName)
                File.Delete(imagePath);

            Employee employee = db.Employees.Find(id);
            if (employee != null)
                db.Employees.Remove(employee);
        }

        public IEnumerable<Employee> GetEmployeesWithDepartmentsAndPositionsOrderedByName()
        {
            return db.Employees
                .Include(d => d.Department)
                .Include(p => p.Position)
                .OrderBy(e => e.EmployeeFullName);
        }

        public Employee GetSingleEmployeeWithDepartmentAndPositionById(int id)
        {
            return db.Employees
                .Include(d => d.Department)
                .Include(p => p.Position)
                .SingleOrDefault(e => e.EmployeeId == id);
        }

        public IQueryable<Employee> GetEmployeesOrderedByName()
        {
            return db.Employees
                .OrderBy(e => e.EmployeeFullName);
        }

        public IQueryable<Employee> GetEmployeesByOneNameParam(string nameParam)
        {
            return db.Employees
                .Where(e => e.EmployeeFullName.Contains(nameParam));
        }
        public IQueryable<Employee> GetEmployeesByTwoNameParams(string firstParam, string secondParam)
        {
            return db.Employees
                .Where(e =>
                    e.EmployeeFullName.Contains(firstParam) &&
                    e.EmployeeFullName.Contains(secondParam));
        }
        public IQueryable<Employee> GetEmployeesByThreeParams(string firstParam, string secondParam, string thirdParam)
        {
            return db.Employees
                .Where(e =>
                    e.EmployeeFullName.Contains(firstParam) &&
                    e.EmployeeFullName.Contains(secondParam) &&
                    e.EmployeeFullName.Contains(thirdParam));
        }
    }
}
