using Web.Domain.Abstract;

namespace Web.Domain;

public class PermissionEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}