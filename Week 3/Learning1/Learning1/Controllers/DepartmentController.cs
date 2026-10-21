
using Learning1.Data;
using Learning1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .Include(e => e.Employees)
                .ToListAsync();
            return Ok(departments);
        }

        // GET: api/Departments/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound("Department not found");

            return Ok(department);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        // PUT: api/Departments/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id , Department updatedepartment)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound("Department Is Doesn't Exsist");
            }
            department.DepartmentName = updatedepartment.DepartmentName;
            await _context.SaveChangesAsync();
            return Ok("Updated Successfully");
        }

        // DELETE: api/Departments/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound("Department not found");

            bool hasEmployees = await _context.Employees
                .AnyAsync(e => e.DepartmentId == id);

            if (hasEmployees)
                return Conflict("Cannot delete a department that has employees");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok("Department deleted successfully");
        }
    }
}
