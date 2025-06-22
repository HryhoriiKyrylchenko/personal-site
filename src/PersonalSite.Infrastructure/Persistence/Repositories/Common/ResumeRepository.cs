using PersonalSite.Domain.Common.Results;
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

    public async Task<PaginatedResult<Resume>> GetFilteredAsync(int page, int pageSize, bool? isActive, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Resumes.AsQueryable().AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(x => x.UploadedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<Resume>.Success(entities, page, pageSize, total);   
    }
}