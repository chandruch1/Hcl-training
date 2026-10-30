using Employees.Data;
using Employees.Interfaces;
using Employees.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(AppDbContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            _logger.LogInformation("Get ALL Employee Details............");
            var employee = await _context.Employees.AsNoTracking().Include(e => e.Department).ToListAsync();
            _logger.LogInformation($"Total {employee.Count} Is Available ");
            return employee;
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching Employee ById............");
            var EmployeeId = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeId == id);
            _logger.LogInformation($"{EmployeeId?.FullName} Is Fetched Successfully");
            return EmployeeId;
        }
        public async  Task<Employee> CreateAsync(Employee employee)
        {
            _logger.LogInformation("Adding Employess Details................");
            await _context.Employees.AddAsync(employee);
            _logger.LogInformation("Employee Added Succeesfully");
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        public async Task<bool> UpdateAsync(int id ,Employee employee)
        {
            _logger.LogInformation("Updating Employee ............");
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (existingEmployee == null)
            {
                return false;
            }
            existingEmployee.FullName = employee.FullName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.DateOfJoining = employee.DateOfJoining;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.IsActive = employee.IsActive;
            _logger.LogInformation($"Employee {employee.EmployeeId} is updated SuccesFully");
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting Employee .............");
            var DeleteEmployee= await _context.Employees.FindAsync(id);

            if (DeleteEmployee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                return false;
            }
            _context.Employees.Remove(DeleteEmployee);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Employee Deleted Succesfully");
            return true;

        }
    }
}
