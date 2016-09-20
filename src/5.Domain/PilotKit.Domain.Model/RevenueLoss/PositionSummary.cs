using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Model.RevenueLoss
{
    public class PositionSummary
    {
        public string Level { get; set; }
        public int OnsitePositions { get; set; }
        public int OffshorePositions { get; set; }
        public int TotalPositions { get; set; }
        public int LostPositions { get; set; }
    }
}
