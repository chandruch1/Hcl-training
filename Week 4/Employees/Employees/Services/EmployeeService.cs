using Employees.Data;
using Employees.Interfaces;
using Employees.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DepartmentService> _logger;
        public EmployeeService(AppDbContext context, ILogger<DepartmentService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            _logger.LogInformation("Get ALL Employee Details............");
            var employee = await _context.Employees.AsNoTracking().ToListAsync();
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
            return employee;
        }
        public async Task<bool> UpdateAsync(int id ,Employee employee)
        {
            _logger.LogInformation("Updating Employee ............");
            var UpdateEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.DepartmentId == id);
            if (UpdateEmployee == null)
            {
                return false;
            }
            UpdateEmployee?.FullName = employee.FullName;
            UpdateEmployee?.DepartmentId = employee.DepartmentId;
            _logger.LogInformation($"Employee {employee.EmployeeId} is updated SuccesFully");
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
