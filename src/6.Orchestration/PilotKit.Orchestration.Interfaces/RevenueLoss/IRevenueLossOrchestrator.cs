using PilotKit.Domain.Model.Common;
using PilotKit.Domain.Model.RevenueLoss;
using PilotKit.Orchestration.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Orchestration.Interfaces.RevenueLoss
{
    public interface IRevenueLossOrchestrator : IOrchestrator
    {
        void UploadRevenueLoss(string fileName);
        void ExportToExcel();
        IList<Positions> GetRevenueLossDetail(string vertical, string country);
        IList<Summary> GetRevenueLossSummary(string vertical);
        IList<SummaryDetail> GetRevenueLossSummaryDetail(string vertical, string country);
        IList<PositionSummary> GetPositionSummary(string level, string vertical);
        IList<PositionMonthly> GetPositionMonthly(string level, string vertical);
        string ExportSummaryToExcel();
    }
}
