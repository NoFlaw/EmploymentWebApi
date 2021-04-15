using EmploymentWebApi.Data.Entities;
using EmploymentWebApi.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EmploymentWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet, Route("~/api/Department/GetDepartments")]
        public IActionResult Get()
        {
            try
            {
                var departments = _departmentService.GetAllDeparments();

                if (departments == null || !departments.Any())
                    return NotFound();

                return Ok(departments);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex); 
            }
        }

        [HttpGet, Route("~/api/Department/GetDepartmentsAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Department>>> GetAsync()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();

                if (departments == null || !departments.Any())
                {
                    return NotFound();
                }

                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet, Route("~/api/Department/GetDepartment/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var department = _departmentService.GetDeparmentById(id);

                if (department == null)
                    return NotFound(); ;

                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet, Route("~/api/Department/GetDepartmentAsync/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<ActionResult<Department>> GetAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var department = await _departmentService.GetDepartmentByIdAsync(id);

                if (department == null)
                {
                    return NotFound();
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost, Route("~/api/Department/AddDepartment")]
        public IActionResult Post([FromBody] Department department)
        {
            try
            {
                _departmentService.AddDepartment(department);
                return CreatedAtAction(nameof(Get), new { id = department.DepartmentId }, department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost, Route("~/api/Department/AddDepartmentAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Department>> PostAsync([FromBody] Department department)
        {
            try
            {
                await _departmentService.AddDepartmentAsync(department);
                return CreatedAtAction(nameof(Get), new { id = department.DepartmentId }, department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut, Route("~/api/Department/UpdateDepartment")]
        public IActionResult Put([FromBody] Department department)
        {
            try
            {
                if (department == null)
                    return NotFound();

                _departmentService.UpdateDepartment(department);

                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut, Route("~/api/Department/UpdateDepartmentAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Department>> PutAsync([FromBody] Department department)
        {
            try
            {
                if (department == null)
                    return NotFound();

                await _departmentService.UpdateDepartmentAsync(department);

                return Ok(department);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpDelete, Route("~/api/Department/DeleteDepartment/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var department = _departmentService.GetDeparmentById(id);

                if (department == null)
                    return NotFound();

                _departmentService.RemoveDepartment(department);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete, Route("~/api/Department/DeleteDepartmentAsync/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);

                if (department == null)
                    return NotFound();

                var removed = await _departmentService.RemoveDepartmentAsync(department);

                return Ok(removed);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
