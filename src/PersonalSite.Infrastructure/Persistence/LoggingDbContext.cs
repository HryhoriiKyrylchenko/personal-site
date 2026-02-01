using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Infrastructure.Persistence;

public class LoggingDbContext : DbContext
{
    public DbSet<LogEntry> LogEntries { get; set; }

    public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEntry>(eb =>
        {
            eb.HasNoKey();
            eb.ToTable("logs");  
        });
        
        modelBuilder.Entity<LogEntry>()
            .Property(e => e.Exception)
            .HasDefaultValue(string.Empty);
    }
}