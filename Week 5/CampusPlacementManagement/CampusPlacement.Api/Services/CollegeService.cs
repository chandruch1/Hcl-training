using CampusPlacement.Api.DTOs;
using CampusPlacement.Api.Enums;
using CampusPlacement.Api.Interfaces.Repositories;
using CampusPlacement.Api.Interfaces.Services;
using CampusPlacement.Api.Models;

namespace CampusPlacement.Api.Services;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _repository;

    public CollegeService(ICollegeRepository repository)
    {
        _repository = repository;
    }

    // Register College
    public async Task<CollegeResponse?> RegisterAsync(
        CollegeRegistrationRequest request)
    {
        // Check duplicate college code
        bool exists = await _repository.CollegeCodeExistsAsync(
            request.CollegeCode);

        if (exists)
            return null;

        // Create college entity
        var college = new College
        {
            CollegeName = request.CollegeName,
            CollegeCode = request.CollegeCode,
            UniversityName = request.UniversityName,
            OfficialEmail = request.OfficialEmail,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Website = request.Website,
            Status = CollegeStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        // Save college
        await _repository.AddAsync(college);

        // Return response DTO
        return MapToResponse(college);
    }

    // Get All Colleges
    public async Task<List<CollegeResponse>> GetAllAsync()
    {
        var colleges = await _repository.GetAllAsync();

        return colleges.Select(MapToResponse).ToList();
    }

    // Get College By ID
    public async Task<CollegeResponse?> GetByIdAsync(int id)
    {
        var college = await _repository.GetByIdAsync(id);

        if (college == null)
            return null;

        return MapToResponse(college);
    }

    // Get Pending Colleges
    public async Task<List<CollegeResponse>> GetPendingAsync()
    {
        var colleges = await _repository.GetPendingAsync();

        return colleges.Select(MapToResponse).ToList();
    }

    // Convert Entity to Response DTO
    private static CollegeResponse MapToResponse(College college)
    {
        return new CollegeResponse
        {
            CollegeId = college.CollegeId,
            CollegeName = college.CollegeName,
            CollegeCode = college.CollegeCode,
            UniversityName = college.UniversityName,
            OfficialEmail = college.OfficialEmail,
            PhoneNumber = college.PhoneNumber,
            Address = college.Address,
            Website = college.Website,
            Status = college.Status,
            CreatedAt = college.CreatedAt
        };
    }
}