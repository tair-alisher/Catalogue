using System.Collections.Generic;
using System.Linq;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class DivisionRepository : IDivisionRepository
    {
        private IContext db;

        public DivisionRepository(IContext context)
        {
            this.db = context;
        }

        public IEnumerable<Division> GetAll()
        {
            return db.Divisions;
        }

        public Division Get(int id)
        {
            return db.Divisions.Find(id);
        }

        public void Create(Division division)
        {
            db.Divisions.Add(division);
        }

        public void Update(Division division)
        {
            db.SetModified(division);
        }

        public void Delete(int id)
        {
            Division division = db.Divisions.Find(id);
            if (division != null)
                db.Divisions.Remove(division);
        }

        public IEnumerable<Division> GetDivisionsOrderedByName()
        {
            return db.Divisions.OrderBy(d => d.DivisionName);
        }

        public IEnumerable<Division> GetDivisionsByNameParts(string[] nameParts)
        {
            return db.Divisions
                .Where(d =>
                    nameParts
                        .All(d.DivisionName.ToLower().Contains)
                );
        }
    }
}
