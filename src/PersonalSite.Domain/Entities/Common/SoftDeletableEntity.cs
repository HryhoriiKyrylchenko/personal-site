namespace PersonalSite.Domain.Entities.Common;

public class SoftDeletableEntity
{
    public bool IsDeleted { get; set; } = false;
}