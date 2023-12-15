using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum ResposneCode
    {
        [DefaultValue("Success")]
        Success,
        [DefaultValue("Failuer")]
        Failuer,
    }
}
