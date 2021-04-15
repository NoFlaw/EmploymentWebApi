using EmploymentWebApi.Data.Entities;
using EmploymentWebApi.Helper;
using EmploymentWebApi.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EmploymentWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IWebHostEnvironment webHostEnvironment)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet, Route("~/api/Employee/GetEmployees")]
        public IActionResult Get()
        {
            try
            {
                var employees = _employeeService.GetAllEmployees();
                return new OkObjectResult(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet, Route("~/api/Employee/GetEmployeesAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAsync()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();

                if (employees == null)
                {
                    return NotFound();
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet, Route("~/api/Employee/GetEmployee/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var employee = _employeeService.GetEmployeeById(id);

                if (employee == null)
                    return NotFound();

                return new OkObjectResult(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            } 
        }

        [HttpGet, Route("~/api/Employee/GetEmployeeAsync/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> GetAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var employee = await _employeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost, Route("~/api/Employee/AddEmployee")]
        public IActionResult Post([FromBody] Employee employee)
        {
            try
            {
                _employeeService.AddEmployee(employee);
                return CreatedAtAction(nameof(Get), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost, Route("~/api/Employee/AddEmployeeAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> PostAsync([FromBody] Employee employee)
        {
            try
            {
                await _employeeService.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(Get), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut, Route("~/api/Employee/UpdateEmployee")]
        public IActionResult Put([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                    return new NoContentResult();

                _employeeService.UpdateEmployee(employee);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }         
        }

        [HttpPut, Route("~/api/Employee/UpdateEmployeeAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> PutAsync([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                    return NotFound();

                await _employeeService.UpdateEmployeeAsync(employee);

                return Ok(employee);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete, Route("~/api/Employee/DeleteEmployee/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var employee = _employeeService.GetEmployeeById(id);
                
                if (employee == null)
                    return NotFound();

                _employeeService.RemoveEmployee(employee);
                
                return new OkResult();
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex); 
            }
        }

        [HttpDelete, Route("~/api/Employee/DeleteEmployeeAsync/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                    return NotFound();

                var removed = await _employeeService.RemoveEmployeeAsync(employee);

                return Ok(removed);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost, Route("~/api/Employee/SaveFile")]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var fileName = postedFile.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [HttpGet, Route("~/api/Employee/GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            try
            {
                var departmentNames = _departmentService.GetAllDeparments().Select(c => new { c.DepartmentName });
                return new JsonResult(departmentNames);
            }
            catch (Exception ex)
            {
                return new JsonErrorResult(new { message = ex?.Message + " " + ex?.InnerException?.Message  });
            }
        }


        [HttpGet, Route("~/api/Employee/GetAllDepartmentNamesAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllDepartmentNamesAsync()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();

                if (departments == null || !departments.Any())
                {
                    return NotFound();
                }

                return Ok(departments.Select(x => new { x.DepartmentName }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
