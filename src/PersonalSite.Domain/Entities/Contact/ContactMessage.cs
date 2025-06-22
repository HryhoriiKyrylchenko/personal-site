namespace PersonalSite.Domain.Entities.Contact;

[Table("ContactMessages")]
public class ContactMessage
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;
    [Required]
    public string Message { get; set; } = string.Empty;
    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;
    [MaxLength(255)]
    public string UserAgent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}