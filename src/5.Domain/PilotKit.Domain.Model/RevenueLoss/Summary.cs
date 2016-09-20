using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Model.RevenueLoss
{
    public class Summary
    {
        public string Organization { get; set; }
        public double RevenueLossAsOfDate { get; set; }
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public double Q3 { get; set; }
        public double Q4 { get; set; }
        public double TotalProjectedRevenueLost { get; set; }
    }
}
