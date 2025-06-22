using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class ResumeRepository : EfRepository<Resume>, IResumeRepository
{
    public ResumeRepository(
        ApplicationDbContext context, 
        ILogger<ResumeRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<Resume?> GetLastActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Resumes
            .OrderByDescending(r => r.UploadedAt)
            .FirstOrDefaultAsync(r => r.IsActive, cancellationToken);
    }
}