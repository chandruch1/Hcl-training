
using Employees.Models;

namespace Employees.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department> CreateAsync(Department department);

        Task<bool> UpdateAsync(int id, Department department);

        Task<bool> DeleteAsync(int id);
        
    }
}
