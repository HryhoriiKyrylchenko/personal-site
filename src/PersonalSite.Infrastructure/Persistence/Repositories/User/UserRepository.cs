using PersonalSite.Domain.Interfaces.Repositories.User;

namespace PersonalSite.Infrastructure.Persistence.Repositories.User;

public class UserRepository : EfRepository<Domain.Entities.User.User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext, ILogger<EfRepository<Domain.Entities.User.User>> logger, IServiceProvider serviceProvider) : base(dbContext, logger, serviceProvider)
    {
    }

    public async Task<Domain.Entities.User.User?> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(id));
        
        return await DbContext.Users
            .Where(u => u.Id == id)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}