using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.DataTransferObject
{
    public class PackageDTO
    {
        public string PackageId { get; set; }
        public List<string> CardIds { get; set; }
    }
}
