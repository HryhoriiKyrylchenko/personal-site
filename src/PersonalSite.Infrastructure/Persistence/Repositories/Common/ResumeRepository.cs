namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class ResumeRepository : EfRepository<Resume>, IResumeRepository
{
    public ResumeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Resume?> GetLastActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Resumes
            .OrderByDescending(r => r.UploadedAt)
            .FirstOrDefaultAsync(r => r.IsActive, cancellationToken);
    }
}