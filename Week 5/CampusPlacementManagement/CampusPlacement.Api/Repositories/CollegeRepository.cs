using CampusPlacement.Api.Data;
using CampusPlacement.Api.Enums;
using CampusPlacement.Api.Interfaces.Repositories;
using CampusPlacement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacement.Api.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _context;

    public CollegeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add a new college
    public async Task AddAsync(College college)
    {
        await _context.Colleges.AddAsync(college);
        await _context.SaveChangesAsync();
    }

    // Get all colleges
    public async Task<List<College>> GetAllAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .OrderBy(c => c.CollegeName)
            .ToListAsync();
    }

    // Get college by ID
    public async Task<College?> GetByIdAsync(int id)
    {
        return await _context.Colleges
            .FirstOrDefaultAsync(c => c.CollegeId == id);
    }

    // Get pending colleges
    public async Task<List<College>> GetPendingAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .Where(c => c.Status == CollegeStatus.Pending)
            .ToListAsync();
    }

    // Update college
    public async Task UpdateAsync(College college)
    {
        _context.Colleges.Update(college);
        await _context.SaveChangesAsync();
    }

    // Check duplicate college code
    public async Task<bool> CollegeCodeExistsAsync(string code)
    {
        return await _context.Colleges
            .AnyAsync(c => c.CollegeCode == code);
    }
}