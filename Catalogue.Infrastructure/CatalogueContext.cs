using System.Data.Entity;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Infrastructure
{
    public class CatalogueContext : DbContext, IContext
    {
        public CatalogueContext() : base("CatalogueContext") { }
        public CatalogueContext(string connectionString) : base(connectionString) { }

        public IDbSet<Administration> Administrations { get; set; }
        public IDbSet<Department> Departments { get; set; }
        public IDbSet<Position> Positions { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Division> Divisions { get; set; }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            SaveChanges();
        }

        public void DbDispose()
        {
            Dispose();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasMany<Employee>(g => g.Employees)
                .WithRequired(s => s.Position)
                .HasForeignKey<int>(s => s.PositionId);
            modelBuilder.Entity<Department>()
                .HasMany<Employee>(g => g.Employees)
                .WithRequired(s => s.Department)
                .HasForeignKey<int>(s => s.DepartmentId);
            modelBuilder.Entity<Administration>()
                .HasMany<Department>(g => g.Departments)
                .WithRequired(s => s.Administration)
                .HasForeignKey<int>(s => s.AdministrationId);
            modelBuilder.Entity<Division>()
                .HasMany<Administration>(g => g.Administrations)
                .WithRequired(s => s.Division)
                .HasForeignKey<int>(s => s.DivisionId);
        }
    }
}
