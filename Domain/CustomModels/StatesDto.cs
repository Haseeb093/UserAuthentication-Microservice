using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomModels
{
    public class StatesDto
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
