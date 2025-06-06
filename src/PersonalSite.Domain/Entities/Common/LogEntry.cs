namespace PersonalSite.Domain.Entities.Common;

[Table("Logs")]
public class LogEntry
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    public LogLevel Level { get; set; }

    [Required, MaxLength(100)]
    public string Source { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Exception { get; set; } 
}