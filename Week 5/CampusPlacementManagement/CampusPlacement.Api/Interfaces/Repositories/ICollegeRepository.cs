using CampusPlacement.Api.Models;

namespace CampusPlacement.Api.Interfaces.Repositories;

public interface ICollegeRepository
{
    Task AddAsync(College college);

    Task<List<College>> GetAllAsync();

    Task<College?> GetByIdAsync(int id);

    Task<List<College>> GetPendingAsync();

    Task UpdateAsync(College college);

    Task<bool> CollegeCodeExistsAsync(string code);
}