namespace PersonalSite.Domain.Interfaces.Repositories.User;

public interface IUserRepository : IRepository<Entities.User.User>
{
    Task<Entities.User.User?> GetById(Guid id, CancellationToken cancellationToken = default); 
}