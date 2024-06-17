using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherizationWorkOrder.Daata.Models.Parents
{
    public class Auditable
    {
        public DateTime? LastModified { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
    }
}
