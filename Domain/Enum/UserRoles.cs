

using System.ComponentModel;

namespace Domain.Enum
{
    public enum UserRoles
    {
        [DefaultValue("Admin")]
        Admin,
        [DefaultValue("User")]
        User,
    }
}
