using Employees.Data;
using Employees.Interfaces;
using Employees.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Employees.DTO;

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
        public async Task<List<EmployeeResponseDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching All EMployess........");
            var Employees= await _context.Employees
                .AsNoTracking()
                .Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfJoining = e.DateOfJoining,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department!.DepartmentName,
                    Role = e.Role,
                    IsActive = e.IsActive
                })
                .ToListAsync();
            _logger.LogInformation($"total Employee {Employees.Count} is Available");
            return Employees;
        }
        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fectching Employeed ById................");
            var Employees= await _context.Employees
                .AsNoTracking()
                .Where(e => e.EmployeeId == id)
                .Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfJoining = e.DateOfJoining,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department!.DepartmentName,
                    Role = e.Role,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();
            _logger.LogInformation($"{Employees?.FullName} Is Fetched Successfully");
            return Employees;
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
        public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto employee)
        {
            var FindId = await _context.Employees.FindAsync(id);
            if (FindId == null)
            {
                _logger.LogInformation("Updation Id is Not Available in Database");
                return false;
            }
            _logger.LogInformation("Updating Employee...");

            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (existingEmployee == null)
            {
                return false;
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

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

            var newValues = JsonSerializer.Serialize(new
            {
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
