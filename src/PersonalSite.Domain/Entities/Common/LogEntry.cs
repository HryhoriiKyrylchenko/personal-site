namespace PersonalSite.Domain.Entities.Common;

[Table("Logs")]
public class LogEntry
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required, MaxLength(128)]
    public string Level { get; set; } = null!;

    [Required]
    public string Message { get; set; } = null!;

    [Required]
    public string MessageTemplate { get; set; } = null!;

    public string? Exception { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Properties { get; set; }

    [MaxLength(255)]
    public string? SourceContext { get; set; } 
}