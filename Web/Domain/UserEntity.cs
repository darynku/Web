using Web.Domain.Abstract;

namespace Web.Domain;

public class UserEntity : BaseEntity
{
    public UserEntity() { }
    
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public List<PermissionEntity> Permissions { get; set; } = [];
}
