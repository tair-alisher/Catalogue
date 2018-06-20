using System.Linq;
using System.Collections.Generic;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class PositionRepository : IPositionRepository
    {
        private IContext db;

        public PositionRepository(IContext context)
        {
            this.db = context;
        }

        public IEnumerable<Position> GetAll()
        {
            return db.Positions;
        }

        public Position Get(int id)
        {
            return db.Positions.Find(id);
        }

        public void Create(Position position)
        {
            db.Positions.Add(position);
        }

        public void Update(Position position)
        {
            db.SetModified(position);
        }

        public void Delete(int id)
        {
            Position position = db.Positions.Find(id);
            if (position != null)
                db.Positions.Remove(position);
        }

        public IEnumerable<Position> GetPositionsOrderedByName()
        {
            return db.Positions
                .OrderBy(p => p.PositionName);
        }

        public IEnumerable<Position> GetPositionsByNameParts(string[] nameParts)
        {
            return db.Positions
                .Where(p =>
                    nameParts.All(p.PositionName.ToLower().Contains)
                );
        }
    }
}
