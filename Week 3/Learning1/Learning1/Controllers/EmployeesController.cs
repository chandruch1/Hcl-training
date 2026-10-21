using Learning1.Data;
using Learning1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Learning1.DTO;
using Learning1.Sevices;

namespace Learning1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;
        public EmployeesController(EmployeeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var res = await _service.GetEmployees();
            if (res == null)
            {
                return NotFound("Employees is Doesn't Exsist");
            }
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            var employee = await _service.GetEmployeeById(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployees([FromBody] CreateEmployeeDto dto)
        {
            return  Ok( await _service.AddEmployees(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id,[FromBody] UpdateEmployeeDto updatedemployee)
        {
            var employee = await _service.UpdateEmployee(id, updatedemployee);
            if(employee == null)
            {
                return NotFound("Employee Not Found");
            }
            return Ok("Employee Updated Succesfully");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var employee = await _service.DeleteEmployee(id);
            if (employee == null)
            {
                return NotFound("Emloyee doesn't Exsist");
            }
            return Ok("Emplyee Delete SuccessFullly");
        }

        [HttpGet("test-success")]
        public IActionResult TestSuccess()
        {
            return Ok(new { message = "Request successful" });
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Test exception occurred");
        }


    }
}
