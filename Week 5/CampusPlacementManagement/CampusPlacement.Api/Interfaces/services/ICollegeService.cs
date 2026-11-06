using CampusPlacement.Api.DTOs;

namespace CampusPlacement.Api.Interfaces.Services;

public interface ICollegeService
{
    Task<CollegeResponse?> RegisterAsync(
        CollegeRegistrationRequest request);

    Task<List<CollegeResponse>> GetAllAsync();

    Task<CollegeResponse?> GetByIdAsync(int id);

    Task<List<CollegeResponse>> GetPendingAsync();
}