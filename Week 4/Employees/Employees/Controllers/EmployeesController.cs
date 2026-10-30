using Microsoft.AspNetCore.Mvc;
using Employees.Interfaces;
using Employees.Models;


namespace Employees.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _services;
        public EmployeesController(IEmployeeService service)
        {
            _services = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAll()
        {
            var employee= await _services.GetAllAsync();
            return Ok(employee);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById([FromRoute] int id)
        {
            var employee = await _services.GetByIdAsync(id);
            if(employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return Ok(employee);
        }
        [HttpPost] 
        public async Task<ActionResult<Employee>> Create([FromBody] Employee employee)
        {
            var createdEmployee = await _services.CreateAsync(employee);
            return CreatedAtAction(
                nameof(GetById),
                new { id = createdEmployee.EmployeeId },
                createdEmployee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest("Employee ID mismatch.");
            }
            var UpdateEmployee = await _services.UpdateAsync(id, employee);
            if (!UpdateEmployee)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var res = await _services.DeleteAsync(id);
            if (!res)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return NoContent();
        }
    }
}
