using Employees.Data;
using Employees.Interfaces;
using Employees.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            await using var transaction =await _context.Database.BeginTransactionAsync();
            _logger.LogInformation("Adding Employess Details................");
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Employee Added Succeesfully");
            var auditLog = new EmployeeAuditLog
            {
                EmployeeId = employee.EmployeeId,
                Action = "CREATE",

                NewValues = JsonSerializer.Serialize(new
                {
                    employee.FullName,
                    employee.Email,
                    employee.Phone,
                    employee.Salary,
                    employee.DepartmentId,
                    employee.IsActive
                }),

                ChangedBy = "System",
                ChangedAt = DateTime.UtcNow
            };
            _context.EmployeeAuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation(
                "Employee {EmployeeId} created and audit logged.",
                employee.EmployeeId);
            return employee;
        }
        public async Task<bool> UpdateAsync(int id ,Employee employee)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            _logger.LogInformation("Updating Employee ............");
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (existingEmployee == null)
            {
                return false;
            }
            var oldValues = JsonSerializer.Serialize(new
            {
                existingEmployee.FullName,
                existingEmployee.Email,
                existingEmployee.Phone,
                existingEmployee.DateOfJoining,
                existingEmployee.Salary,
                existingEmployee.DepartmentId,
                existingEmployee.IsActive
            });

            existingEmployee.FullName = employee.FullName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.DateOfJoining = employee.DateOfJoining;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.IsActive = employee.IsActive;
            

            var newValues = JsonSerializer.Serialize(new {
                existingEmployee.FullName,
                existingEmployee.Email,
                existingEmployee.Phone,
                existingEmployee.DateOfJoining,
                existingEmployee.Salary,
                existingEmployee.DepartmentId,
                existingEmployee.IsActive
            });
            var auditLog = new EmployeeAuditLog
            {
                EmployeeId = id,
                Action = "UPDATE",
                OldValues = oldValues,
                NewValues = newValues,
                ChangedBy = "System",
                ChangedAt = DateTime.UtcNow
            };
            _context.EmployeeAuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"Employee {employee.EmployeeId} is updated SuccesFully");
            _logger.LogInformation(
                "Employee {EmployeeId} updated and audit logged.",
                id);

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

        public async Task<List<Employee>> SearchByNameAsync(string name)
        {
            var EmployeeByName = await _context.Employees.Where(e => e.FullName.Contains(name)).
                AsNoTracking().ToListAsync();
            return EmployeeByName;
        }
    }
}
