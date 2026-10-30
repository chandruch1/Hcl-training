using Employees.DTO;
using Employees.Models;

namespace Employees.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllAsync();
        Task<EmployeeResponseDto?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<bool> UpdateAsync(int id, UpdateEmployeeDto employee);
        Task<bool> DeleteAsync(int id);
        Task<List<Employee>> SearchByNameAsync(string Name);
    }
}
