using Ninject.Modules;
using Catalogue.Core;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IContext>().To<CatalogueContext>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IAdministrationRepository>().To<AdministrationRepository>();
            Bind<IDepartmentRepository>().To<DepartmentRepository>();
            Bind<IDivisionRepository>().To<DivisionRepository>();
            Bind<IEmployeeRepository>().To<EmployeeRepository>();
            Bind<IPositionRepository>().To<PositionRepository>();
        }
    }
}