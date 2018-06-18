using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private IContext db;

        public DepartmentRepository(IContext context)
        {
            this.db = context;
        }

        public IEnumerable<Department> GetAll()
        {
            return db.Departments;
        }

        public Department Get(int id)
        {
            return db.Departments.Find(id);
        }

        public void Create(Department department)
        {
            db.Departments.Add(department);
        }

        public void Update(Department department)
        {
            db.SetModified(department);
        }

        public void Delete(int id)
        {
            Department department = db.Departments.Find(id);
            if (department != null)
                db.Departments.Remove(department);
        }

        public IEnumerable<Department> GetDepartmentsWithAdministrationsOrderedByName()
        {
            return db.Departments
                .Include(e => e.Administration)
                .OrderBy(i => i.DepartmentName);
        }

        public Department GetSingleDepartmentWithAdministrationById(int id)
        {
            return db.Departments
                .Include(e => e.Administration)
                .SingleOrDefault(d => d.DepartmentId == id);
        }
    }
}
