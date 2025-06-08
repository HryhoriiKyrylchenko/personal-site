using PersonalSite.Application.Services.Aggregates.DTOs;

namespace PersonalSite.Application.Services.Aggregates;

public interface IPagesDataService
{
    Task<HomePageDto?> GetHomePageAsync(CancellationToken cancellationToken);
    Task<AboutPageDto?> GetAboutPageAsync(CancellationToken cancellationToken);
    Task<PortfolioPageDto?> GetPortfolioPageAsync(CancellationToken cancellationToken);
    Task<BlogPageDto?> GetBlogPageAsync(CancellationToken cancellationToken);
    Task<ContactPageDto?> GetContactPageAsync(CancellationToken cancellationToken);
}