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
    public interface IDepartmentService : IGenericRepository<Department>
    {
        Department GetDeparmentById(int departmentId);
        Task<Department> GetDepartmentByIdAsync(int departmentId);
        Department AddDepartment(Department department);
        Task<Department> AddDepartmentAsync(Department department);
        bool RemoveDepartment(Department department);
        Task<bool> RemoveDepartmentAsync(Department department);
        Department UpdateDepartment(Department department);
        Task<Department> UpdateDepartmentAsync(Department department);
        IQueryable<Department> GetAllDeparments();
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
