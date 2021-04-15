using EmploymentWebApi.Data.Entities;
using EmploymentWebApi.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentWebApi.Service
{
    //Custom entity interface inherits from generic repository interface
    //If you need any specific action/customization for an entity it here.
    public class EmployeeService: GenericRepository<Employee>, IEmployeeService
    {
        public EmployeeService(AppDbContext context) : base(context)
        {
        }

        public Employee GetEmployeeById(int employeeId)
        {
            //This would also work, just showcasing..
            //var employee = Get(employeeId); 
            var employee = GetQuery().FirstOrDefault(e => e.EmployeeId == employeeId);
            return employee;
        }

        public Employee AddEmployee(Employee employee)
        {            
            if (employee == null)
                throw new Exception("Employee cannot be null");

            if (string.IsNullOrEmpty(employee?.EmployeeName))
                throw new Exception("Employee Name cannot be null");

            return base.Add(employee);
        }

        public bool RemoveEmployee(Employee employee)
        {
            if (employee == null || employee.EmployeeId <= 0)
                return false;

            base.Delete(employee);

            return true;
        }

        public Employee UpdateEmployee(Employee employee)
        {            
            return base.Update(employee, employee.EmployeeId);
        }

        //Example of Custom Implementation of IAuditable Logic
        //public override Employee Update(Employee t, object key)
        //{
        //    Employee exist = _context.Set<Employee>().Find(key);

        //    if (exist != null)
        //    {
        //        t.CreatedBy = exist.CreatedBy;
        //        t.CreatedOn = exist.CreatedOn;
        //    }

        //    return base.Update(t, key);
        //}

        public IQueryable<Employee> GetAllEmployees()
        {
            return GetQuery();
        }

        public int SaveChanges()
        {
            return base.Save();
        }

        #region Asynchronous Methods

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await GetQuery().Include(x => x.Department).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            return await GetQuery().Include(x => x.Department).FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            if (employee == null)
                throw new Exception("Employee cannot be null");

            if (string.IsNullOrEmpty(employee?.EmployeeName))
                throw new Exception("Employee Name cannot be null");

            return await base.AddAsync(employee);
        }

        public async Task<bool> RemoveEmployeeAsync(Employee employee)
        {
            if (employee == null || employee.EmployeeId <= 0)
                return false;

            return await base.DeleteAsync(employee) > 0;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            return await base.UpdateAsync(employee, employee.EmployeeId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveAsync();
        }

        #endregion

    }
}
