

using System.ComponentModel;

namespace Domain.Enum
{
    public enum Roles
    {
        [DefaultValue("Admin")]
        Admin,
        [DefaultValue("Doctor")]
        Doctor,
        [DefaultValue("Patient")]
        Patient,
        [DefaultValue("Staff")]
        Staff
    }
}
