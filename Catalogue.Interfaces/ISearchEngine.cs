using System.Collections.Generic;
using System.Linq;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface ISearchEngine
    {
        IQueryable<Employee> EmployeeSearch(string name, int? positionId, int? departmentId, int? administrationId, int? divisionId);
        IEnumerable<Department> DepartmentSearch(string[] nameParts);
        IEnumerable<Administration> AdministrationSearch(string[] nameParts);
    }
}
