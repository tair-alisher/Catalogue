using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface IDivisionRepository
    {
        IEnumerable<Division> GetAll();
        Division Get(int id);
        void Create(Division division);
        void Update(Division division);
        void Delete(int id);
        IEnumerable<Division> GetDivisionsOrderedByName();
    }
}
