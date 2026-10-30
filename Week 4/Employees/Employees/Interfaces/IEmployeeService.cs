using Employees.Models;

namespace Employees.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<bool> UpdateAsync(int id,Employee employee);
        Task<bool> DeleteAsync(int id);
        Task<List<Employee>> SearchByNameAsync(string Name);
    }
}
