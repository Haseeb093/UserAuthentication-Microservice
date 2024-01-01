using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomModels
{
    public class CitiesDto
    {
        public int CityId { get; set; }
        public string Name { get; set; }
    }
}
