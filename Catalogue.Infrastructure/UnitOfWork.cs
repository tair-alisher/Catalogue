using System;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IContext db;

        public UnitOfWork()
        {
            db = new CatalogueContext();
        }

        public UnitOfWork(string connectionString)
        {
            db = new CatalogueContext(connectionString);
        }

        private IAdministrationRepository administrationRepo;
        private IDepartmentRepository departmentRepo;
        private IDivisionRepository divisionRepo;
        private IEmployeeRepository employeeRepo;
        private IPositionRepository positionRepo;

        public IAdministrationRepository Administrations
        {
            get
            {
                if (administrationRepo == null)
                    administrationRepo = new AdministrationRepository(db);
                return administrationRepo;
            }
        }

        public IDepartmentRepository Departments
        {
            get
            {
                if (departmentRepo == null)
                    departmentRepo = new DepartmentRepository(db);
                return departmentRepo;
            }
        }

        public IDivisionRepository Divisions
        {
            get
            {
                if (divisionRepo == null)
                    divisionRepo = new DivisionRepository(db);
                return divisionRepo;
            }
        }

        public IEmployeeRepository Employees
        {
            get
            {
                if (employeeRepo == null)
                    employeeRepo = new EmployeeRepository(db);
                return employeeRepo;
            }
        }

        public IPositionRepository Positions
        {
            get
            {
                if (positionRepo == null)
                    positionRepo = new PositionRepository(db);
                return positionRepo;
            }
        }

        public void Save() { db.Save(); }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.DbDispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
