using System;
using Microsoft.AspNetCore.Mvc;
using Employees.Services;
using Employees.Interfaces;
using Employees.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Employees.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();

            return Ok(departments);
        }
        [HttpGet("{id}")] 
        public async Task<ActionResult<Department?>> GetById([FromRoute] int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new { Message="NO departments Found" });

            }
            return Ok(department);
        }
        [HttpPost]
        public async Task<ActionResult<Department?>> Creating(Department department)
        {
            var createdDepartment = await _departmentService.CreateAsync(department);
            return CreatedAtAction(nameof(GetById),
                new { id = createdDepartment.DepartmentId },
                createdDepartment);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Updating(int id,Department department)
        {
            var UpdateDepartment = await _departmentService.UpdateAsync(id, department);
            if (!UpdateDepartment)
            {
                return NotFound(new { Message = "Department NotFound" });
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =await _departmentService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new{ message = "Department not found." });
            }

            return NoContent();
        }

    }
}
