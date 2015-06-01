using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public class ModelBuildInfo
    {
        public int LeadingPrecision { get; set; }

        public int TrailingPrecision { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }
    }
}
