using System;

namespace Catalogue.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAdministrationRepository Administrations { get; }
        IDepartmentRepository Departments { get; }
        IDivisionRepository Divisions { get; }
        IEmployeeRepository Employees { get; }
        IPositionRepository Positions { get; }
        void Save();
    }
}
