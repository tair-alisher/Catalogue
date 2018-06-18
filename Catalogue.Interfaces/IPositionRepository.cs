using System;
using System.Collections.Generic;
using System.Linq;
using Catalogue.Core;

namespace Catalogue.Interfaces
{
    public interface IPositionRepository
    {
        IEnumerable<Position> GetAll();
        Position Get(int id);
        void Create(Position position);
        void Update(Position position);
        void Delete(int id);

        IEnumerable<Position> GetPositionsOrderedByName();
    }
}
