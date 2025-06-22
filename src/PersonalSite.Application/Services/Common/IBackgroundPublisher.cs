namespace PersonalSite.Application.Services.Common;

public interface IBackgroundPublisher
{
    void Schedule<T>(T command, DateTime executeAtUtc)
        where T : class;
}
