using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class AdministrationRepository : IAdministrationRepository
    {
        private IContext db;

        public AdministrationRepository(IContext context)
        {
            this.db = context;
        }

        public IEnumerable<Administration> GetAll()
        {
            return db.Administrations;
        }

        public Administration Get(int id)
        {
            return db.Administrations.Find(id);
        }

        public void Create(Administration administration)
        {
            db.Administrations.Add(administration);
        }

        public void Update(Administration administration)
        {
            db.SetModified(administration);
        }

        public void Delete(int id)
        {
            Administration administration = db.Administrations.Find(id);
            if (administration != null)
                db.Administrations.Remove(administration);
        }

        public IEnumerable<Administration> GetAdministrationsWithDivisionsOrderedByName()
        {
            return db.Administrations
                .Include(e => e.Division)
                .OrderBy(i => i.AdministrationName);
        }

        public Administration GetSingleAdministrationWithDivisionById(int id)
        {
            return db.Administrations
                .Include(e => e.Division)
                .SingleOrDefault(d => d.AdministrationId == id);
        }
    }
}
