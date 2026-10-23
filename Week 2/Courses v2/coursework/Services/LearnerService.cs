using CoursesWebApp.Data;
using CoursesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebApp.Services;

public class LearnerService(ApplicationDbContext db)
{
    public async Task<List<Learner>> GetAllAsync() => await db.Learners.AsNoTracking().OrderBy(l => l.LearnerId).ToListAsync();
    public async Task<Learner?> FindByIdAsync(int id) => await db.Learners.FirstOrDefaultAsync(l => l.LearnerId == id);

    public async Task<Learner> AddAsync(RegisterLearnerRequest request)
    {
        if (request.LearnerId <= 0) throw new ArgumentException("Learner ID must be positive.");
        if (string.IsNullOrWhiteSpace(request.LearnerName)) throw new ArgumentException("Learner name is required.");
        if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentException("Email is required.");
        if (await db.Learners.AnyAsync(l => l.LearnerId == request.LearnerId)) throw new ArgumentException("Learner ID already exists.");
        if (await db.Learners.AnyAsync(l => l.Email == request.Email.Trim())) throw new ArgumentException("Email is already registered.");

        var learner = new Learner { LearnerId = request.LearnerId, LearnerName = request.LearnerName.Trim(), Email = request.Email.Trim(), Contact = request.Contact };
        db.Learners.Add(learner);
        await db.SaveChangesAsync();
        return learner;
    }
}
