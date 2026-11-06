using CampusPlacement.Api.DTOs;

namespace CampusPlacement.Api.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}