using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Catalogue.Core;
using Catalogue.Interfaces;

namespace Catalogue.Services
{
    public class SearchEngine : IDisposable, ISearchEngine
    {
        private IUnitOfWork unit;
        private const int MaxNumberOfWordsInFullName = 3;

        public SearchEngine(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        public IQueryable<Employee> EmployeeSearch(string name, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            IQueryable<Employee> employees = Enumerable.Empty<Employee>().AsQueryable();

            if (name.Length <= 0)
                employees = unit
                    .Employees
                    .GetEmployeesOrderedByName();
            else
                employees = SplitNameAndBuildQuery(name);

            employees = FilterEmployees(employees, positionId, departmentId, administrationId, divisionId);

            employees = AddRelationsToEmployees(employees);

            return employees;
        }

        public IEnumerable<Administration> AdministrationSearch(string[] nameParts)
        {
            return BuildAdministrationSearchQuery(nameParts);
        }
        public IEnumerable<Department> DepartmentSearch(string[] nameParts)
        {
            return BuildDepartmentSearchQuery(nameParts);
        }
        public IEnumerable<Division> DivisionSearch(string[] nameParts)
        {
            return BuildDivisionSearchQuery(nameParts);
        }
        public IEnumerable<Position> PositionSearch(string[] nameParts)
        {
            return BuildPositionSearchQuery(nameParts);
        }
        
        private IQueryable<Employee> SplitNameAndBuildQuery(string name)
        {
            IQueryable<Employee> employees = Enumerable.Empty<Employee>().AsQueryable();

            Stack<string> words = new Stack<string>();

            string[] inputWords = name.Split(' ');

            int wordsAmount = inputWords.Length < MaxNumberOfWordsInFullName ? inputWords.Length : MaxNumberOfWordsInFullName;

            for (int i = 0; i < wordsAmount; i++)
                words.Push(inputWords[i]);

            if (words.Count == 1)
            {
                string part_1 = words.Pop();
                employees = BuildSearchQuery(part_1);
            }
            else if (words.Count == 2)
            {
                string part_1 = words.Pop(), part_2 = words.Pop();
                employees = BuildSearchQuery(part_1, part_2);
            }
            else if (words.Count == 3)
            {
                string part_1 = words.Pop(),
                    part_2 = words.Pop(),
                    part_3 = words.Pop();
                employees = BuildSearchQuery(part_1, part_2, part_3);
            }

            return employees;
        }

        private IQueryable<Employee> FilterEmployees(IQueryable<Employee> query, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            if (positionId != null)
                query = query.Where(e => e.PositionId == positionId);
            if (departmentId != null)
                query = query.Where(e => e.DepartmentId == departmentId);
            if (administrationId != null)
            {
                List<int> departmentIds = GetDepartmentIds("administration", administrationId);
                query = query.Where(e => departmentIds.Contains(e.DepartmentId));
            }
            if (divisionId != null)
            {
                List<int> departmentIds = GetDepartmentIds("division", divisionId);
                query = query.Where(e => departmentIds.Contains(e.DepartmentId));
            }

            return query;
        }

        private IQueryable<Employee> AddRelationsToEmployees(IQueryable<Employee> employees)
        {
            return employees
                .OrderBy(e => e.EmployeeFullName)
                .Include(d => d.Department)
                .Include(e => e.Department.Administration)
                .Include(p => p.Position);
        }

        private IEnumerable<Administration> BuildAdministrationSearchQuery(string[] nameParts)
        {
            return unit.Administrations
                .GetAdministrationsByNameParts(nameParts);
        }
        private IEnumerable<Department> BuildDepartmentSearchQuery(string[] nameParts)
        {
            return unit.Departments
                .GetDepartmentsByNameParts(nameParts);
        }
        private IEnumerable<Division> BuildDivisionSearchQuery(string[] nameParts)
        {
            return unit.Divisions
                .GetDivisionsByNameParts(nameParts);
        }
        private IEnumerable<Position> BuildPositionSearchQuery(string[] nameParts)
        {
            return unit.Positions
                .GetPositionsByNameParts(nameParts);
        }

        private List<int> GetDepartmentIds(string type, int? id)
        {
            List<int> departmentIds = new List<int>();

            if (type == "administration")
                departmentIds = GetDepartmentIdsByAdministrationId(id);
            else if (type == "division")
                departmentIds = GetDepartmentIdsByDivision(id);

            return departmentIds;
        }

        private List<int> GetDepartmentIdsByAdministrationId(int? id)
        {
            return unit.Departments
                .GetDepartmentIdsByAdministrationID((int)id)
                .ToList();
        }

        private List<int> GetDepartmentIdsByDivision(int? id)
        {
            List<int> administrationIds = unit.Administrations
                .GetAdministrationIdsByDivisionId((int)id)
                .ToList();

            return unit.Departments
                .GetDepartmentIdsByAdministrationIds(administrationIds)
                .ToList();
        }

        private IQueryable<Employee> BuildSearchQuery(string firstParam)
        {
            return unit.Employees
                .GetEmployeesByOneNameParam(firstParam);
        }
        private IQueryable<Employee> BuildSearchQuery(string firstParam, string secondParam)
        {
            return unit.Employees
                .GetEmployeesByTwoNameParams(firstParam, secondParam);
        }
        private IQueryable<Employee> BuildSearchQuery(string firstParam, string secondParam, string thirdParam)
        {
            return unit.Employees
                .GetEmployeesByThreeParams(firstParam, secondParam, thirdParam);
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
