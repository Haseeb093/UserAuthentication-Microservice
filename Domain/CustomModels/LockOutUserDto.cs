using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomModels
{
    public class LockOutUserDto
    {
        public string UpdateByUser { get; set; }
        public string UserId { get; set; }
    }
}
