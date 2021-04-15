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
    public class DepartmentService : GenericRepository<Department>, IDepartmentService
    {
        public DepartmentService(AppDbContext context) : base(context)
        {
        }

        public Department GetDeparmentById(int departmentId)
        {
            //This would also work, just showcasing..
            //var department = Get(departmentId); 
            var department = GetQuery().Include(x => x.Employees).FirstOrDefault(d => d.DepartmentId == departmentId);
            return department;
        }

        public Department AddDepartment(Department department)
        {
            if (department == null)
                throw new Exception("Department cannot be null");

            return base.Add(department);
        }

        public bool RemoveDepartment(Department department)
        {
            if (department == null || department.DepartmentId <= 0)
                return false;

            return base.Delete(department) > 0;
        }

        public Department UpdateDepartment(Department department)
        {
            return base.Update(department, department.DepartmentId);
        }

        //Example of Custom Implementation of IAuditable Logic
        //public override Department Update(Department t, object key)
        //{
        //    Department exist = _context.Set<Department>().Find(key);

        //    if (exist != null)
        //    {
        //        t.CreatedBy = exist.CreatedBy;
        //        t.CreatedOn = exist.CreatedOn;
        //    }

        //    return base.Update(t, key);
        //}

        public IQueryable<Department> GetAllDeparments()
        {
            return GetQuery();
        }

        public int SaveChanges()
        {
            return base.Save();
        }

        #region Asynchronous Methods

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            return await GetAsync(departmentId);
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            if (department == null)
                throw new Exception("Department cannot be null");

            return await base.AddAsync(department);
        }

        public async Task<bool> RemoveDepartmentAsync(Department department)
        {
            if (department == null || department.DepartmentId <= 0)
                return false;

            return await base.DeleteAsync(department) > 0;
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            return await base.UpdateAsync(department, department.DepartmentId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveAsync();
        }

        #endregion
    }
}
