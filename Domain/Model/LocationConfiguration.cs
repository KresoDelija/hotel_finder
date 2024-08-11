using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class LocationConfiguration
    {
        public int SpatialReferenceSystem { get; set; }
        public int MaxPageSize { get; set; } = 10;
    }
}
