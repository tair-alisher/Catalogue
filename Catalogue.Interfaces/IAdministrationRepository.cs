using System.Collections.Generic;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface IAdministrationRepository
    {
        IEnumerable<Administration> GetAll();
        Administration Get(int id);
        void Create(Administration administration);
        void Update(Administration administration);
        void Delete(int id);
        IEnumerable<Administration> GetAdministrationsWithDivisionsOrderedByName();
        Administration GetSingleAdministrationWithDivisionById(int id);
        IEnumerable<int> GetAdministrationIdsByDivisionId(int id);
        IEnumerable<Administration> GetAdministrationsByNameParts(string[] nameParts);
    }
}
