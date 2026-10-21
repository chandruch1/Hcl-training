using Learning1.Controllers;
using Learning1.Data;
using Learning1.DTO;
using Learning1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace Learning1.Sevices
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<string> AddEmployees(CreateEmployeeDto dto)
        {

            if (dto.DepartmentId.HasValue)
            {
                var departmentExists = await _context.Departments
                    .AnyAsync(d => d.Id == dto.DepartmentId.Value);

                if (!departmentExists)
                {
                    return "Department Not Found";
                }
            }
            var employee = new Employee()
            {
                Name = dto.Name,
                DepartmentId = dto.DepartmentId,
                Salary = dto.Salary
            };

            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return "Employee Added Successfully";
            }
            catch(DbException e)
            {
                return e.Message;
            }

        }

        public async Task<string?> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee == null)
            {
                return null;
            }
            employee.Name = dto.Name;
            employee.Salary = dto.Salary;

            await _context.SaveChangesAsync();
            return "Employee Updated Succesfully";
        }

        public async Task<string?> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee == null)
            {
                return null;
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return "Employee Deleted Succesfully";
        }
    }
}
