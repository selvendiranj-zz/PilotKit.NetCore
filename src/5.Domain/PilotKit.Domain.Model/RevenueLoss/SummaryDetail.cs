using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Model.RevenueLoss
{
    public class SummaryDetail
    {
        public string Organization { get; set; }
        public double RevenueLossAsOfDate_CAD { get; set; }
        public double RevenueLossAsOfDate_GBP { get; set; }
        public double RevenueLossAsOfDate_USD { get; set; }
        public double January { get; set; }
        public double February { get; set; }
        public double March { get; set; }
        public double Q1ActualLost_CAD { get; set; }
        public double Q1ActualLost_GBP { get; set; }
        public double Q1ActualLost_USD { get; set; }
        public double April { get; set; }
        public double May { get; set; }
        public double June { get; set; }
        public double Q2ActualLost_CAD { get; set; }
        public double Q2ActualLost_GBP { get; set; }
        public double Q2ActualLost_USD { get; set; }
        public double July { get; set; }
        public double August { get; set; }
        public double September { get; set; }
        public double Q3ActualLost_CAD { get; set; }
        public double Q3ActualLost_GBP { get; set; }
        public double Q3ActualLost_USD { get; set; }
        public double October { get; set; }
        public double November { get; set; }
        public double December { get; set; }
        public double Q4ActualLost_CAD { get; set; }
        public double Q4ActualLost_GBP { get; set; }
        public double Q4ActualLost_USD { get; set; }
    }
}
