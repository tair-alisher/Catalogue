using System.Web;
using System.Collections.Generic;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee Get(int id);
        void Create(Employee employee, HttpPostedFileBase image, string imageDirectory);
        void Update(Employee employee);
        void Delete(int id, string imagePath);

        IEnumerable<Employee> GetEmployeesWithDepartmentsAndPositionsOrderedByName();

        Employee GetSingleEmployeeWithDepartmentAndPositionById(int id);

        void Edit(
            Employee employee,
            HttpPostedFileBase newImage,
            string oldImage,
            string imageDirectory,
            string oldImagePath
            );
    }
}
