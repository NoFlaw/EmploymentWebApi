using System;
using System.Collections.Generic;

#nullable disable

namespace EmploymentWebApi.Data.Entities
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
