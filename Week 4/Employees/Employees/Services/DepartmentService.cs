using Microsoft.EntityFrameworkCore;
using Employees.Models;
using Employees.Data;
using Employees.Interfaces;

namespace Employees.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DepartmentService> _logger;
        public DepartmentService (AppDbContext context, ILogger<DepartmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            _logger.LogInformation("Fectching All DepartmentList.......");
            var department = await _context.Departments.AsNoTracking().ToListAsync();
            _logger.LogInformation($"Totally {department.Count} is Fetched");
            return department;
        }
        public async Task<Department?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching Department by Id .........");
            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.DepartmentId == id);
            _logger.LogInformation($"Fetch {department?.DepartmentId} Is Succesfully");
            return department;

        }
        public async Task<Department> CreateAsync(Department department)
        {
            _logger.LogInformation("Creating new Employee In Database.............");
             _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Saved {department.DepartmentName}  is Successfully Added in Database");
            return (department);
        }

        public async Task<bool> UpdateAsync(int id,Department updatedepartment)
        {
            _logger.LogInformation("Update Students.............");
            var existingDepartment = await _context.Departments.FindAsync(id);
            if (existingDepartment == null)
            {
                return false;
            }
            existingDepartment.DepartmentName = updatedepartment.DepartmentName;
            existingDepartment.Description = updatedepartment.Description;

            await _context.SaveChangesAsync();
            _logger.LogInformation($" {updatedepartment.DepartmentId} is Updated Succesfully ");

            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting Department ............");
            var DeleteDepartment = await _context.Departments.FindAsync(id);
            if (DeleteDepartment == null)
            {
                return false;
            }
             _context.Departments.Remove(DeleteDepartment);
             await _context.SaveChangesAsync();
            _logger.LogInformation($"{DeleteDepartment.DepartmentName} is Deleted Successfully");
            return true;
        }
    }
}
