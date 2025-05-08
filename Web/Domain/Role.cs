using System.ComponentModel;

namespace Web.Domain;

public enum Role
{
    [Description("Админ")]
    Admin = 1,
    
    [Description("Пользотваель")]
    User = 2
}