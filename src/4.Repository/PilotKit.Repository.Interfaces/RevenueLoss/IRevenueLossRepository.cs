using Insight.Database;
using PilotKit.Repository.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PilotKit.Domain.Model.RevenueLoss;
using PilotKit.Domain.Model.Common;

namespace PilotKit.Repository.Interfaces.RevenueLoss
{
    public interface IRevenueLossRepository : IRepository
    {
        void InsertPositions(IList<Positions> positions);
        void CalculateRevenueLoss(int year);
        IList<Positions> GetPositions();
        IList<Positions> GetRevenueLossDetail(string vertical);
        IList<Positions> GetRevenueLossDetail(string vertical, string country);
        void RemoveAllPositions(int year);
        IList<Summary> GetRevenueLossSummary(int year);
        IList<Summary> GetRevenueLossSummary(int year, string vertical);
        IList<SummaryDetail> GetRevenueLossSummaryDetail(int year, string vertical, string country);
        IList<PositionMonthly> GetPositionMonthly(int year, string level);
        IList<PositionMonthly> GetPositionMonthly(int year, string level, string vertical);
        IList<PositionSummary> GetPositionSummary(int year, string level);
        IList<PositionSummary> GetPositionSummary(int year, string level, string vertical);
    }
}
