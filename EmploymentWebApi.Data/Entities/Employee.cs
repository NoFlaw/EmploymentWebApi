using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EmploymentWebApi.Data.Entities
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate { get; set; }
        public string PhotoFileName { get; set; }

        public virtual Department Department { get; set; }
    }
}
