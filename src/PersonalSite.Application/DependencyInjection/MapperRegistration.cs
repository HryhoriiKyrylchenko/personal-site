namespace PersonalSite.Application.DependencyInjection;

public static class MapperRegistration
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(IMapper<,>),
                typeof(ITranslatableMapper<,>),
                typeof(IAdminMapper<,>)
            ))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        return services;
    }
}