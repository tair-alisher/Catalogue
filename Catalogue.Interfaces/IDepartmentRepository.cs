using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        Department Get(int id);
        void Create(Department department);
        void Update(Department department);
        void Delete(int id);
        IEnumerable<Department> GetDepartmentsWithAdministrationsOrderedByName();
        Department GetSingleDepartmentWithAdministrationById(int id);
    }
}
