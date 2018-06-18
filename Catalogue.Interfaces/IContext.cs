using System.Data.Entity;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface IContext
    {
        IDbSet<Administration> Administrations { get; }
        IDbSet<Department> Departments { get; }
        IDbSet<Position> Positions { get; }
        IDbSet<Employee> Employees { get; }
        IDbSet<Division> Divisions { get; }

        void SetModified(object entity);
        void Save();
        void DbDispose();
    }
}
