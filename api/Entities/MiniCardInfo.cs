using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class MiniCardInfo
    {
        public string icon { get; set; }
        public string title { get; set; }
        public decimal value { get; set; }
        public string color { get; set; }
        public bool isIncrease { get; set; }
        public bool isCurrency { get; set; }
        public string duration { get; set; }
        public decimal percentValue { get; set; }
   
    }
}
