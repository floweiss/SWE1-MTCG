using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.DataTransferObject
{
    public class CardDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }
    }
}
