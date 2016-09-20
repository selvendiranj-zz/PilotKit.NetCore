using PilotKit.Domain.Interfaces.Common;
using PilotKit.Domain.Model.RevenueLoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Interfaces.RevenueLoss
{
    public interface IRevenueLossService : IDomainService
    {
        IList<Positions> GetPositions();
        void RemoveAllPositions();
        void UploadPositions(IList<Positions> positions, string sheetName);
        IList<Positions> GetRevenueLossDetail(string vertical, string country);
        IList<Summary> GetRevenueLossSummary(string vertical);
        IList<SummaryDetail> GetRevenueLossSummaryDetail(string vertical, string country);
        IList<PositionSummary> GetPositionSummary(string level, string vertical);
        IList<PositionMonthly> GetPositionMonthly(string level, string vertical);
    }
}
