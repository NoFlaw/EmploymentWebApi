using EmploymentWebApi.Data.Entities;
using EmploymentWebApi.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentWebApi.Service
{
    //Custom entity interface inherits from generic repository interface
    //If you need any specific action for an entity such as id type we can put it here.
    public interface IEmployeeService : IGenericRepository<Employee>
    {
        Employee GetEmployeeById(int employeeId);
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Employee AddEmployee(Employee employee);
        Task<Employee> AddEmployeeAsync(Employee employee);
        bool RemoveEmployee(Employee employee);
        Task<bool> RemoveEmployeeAsync(Employee employee);
        Employee UpdateEmployee(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        IQueryable<Employee> GetAllEmployees();
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
