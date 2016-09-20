using PilotKit.Repository.DataAccess.Common;
using PilotKit.Repository.Interfaces.RevenueLoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insight.Database;
using PilotKit.Domain.Model.RevenueLoss;
using System.Data;

namespace PilotKit.Repository.DataAccess.RevenueLoss
{
    public abstract class RevenueLossRepository : RepositoryBase, IRevenueLossRepository
    {
        public abstract IDbConnection GetConnection();

        public void InsertPositions(IList<Positions> positions)
        {
            this.GetConnection().BulkCopy<Positions>("dbo.Positions", positions);
        }

        [Sql("Select * From Positions")]
        public abstract IList<Positions> GetPositions();

        [Sql("Select * From Positions Where Vertical = @Vertical")]
        public abstract IList<Positions> GetRevenueLossDetail(string vertical);

        [Sql("Select * From Positions Where Vertical = @Vertical And Country = @Country")]
        public abstract IList<Positions> GetRevenueLossDetail(string vertical, string country);

        [Sql("Delete From Positions Where LossAssessmentYear = @Year")]
        public abstract void RemoveAllPositions(int year);

        [Sql("dbo.CalculateRevenueLoss")]
        public abstract void CalculateRevenueLoss(int year);

        [Sql("dbo.GetRevenueLossSummary @Year")]
        public abstract IList<Summary> GetRevenueLossSummary(int year);

        [Sql("dbo.GetRevenueLossSummary @Year, @Vertical")]
        public abstract IList<Summary> GetRevenueLossSummary(int year, string vertical);

        [Sql("dbo.GetRevenueLossSummary @Year, @Vertical, @Country")]
        public abstract IList<SummaryDetail> GetRevenueLossSummaryDetail(int year, string vertical, string country);

        [Sql("dbo.GetPositionSummary @Year, @Level")]
        public abstract IList<PositionSummary> GetPositionSummary(int year, string level);

        [Sql("dbo.GetPositionSummary @Year, @Level, @Vertical")]
        public abstract IList<PositionSummary> GetPositionSummary(int year, string level, string vertical);

        [Sql("dbo.GetPositionMonthly @Year, @Level")]
        public abstract IList<PositionMonthly> GetPositionMonthly(int year, string level);

        [Sql("dbo.GetPositionMonthly @Year, @Level, @Vertical")]
        public abstract IList<PositionMonthly> GetPositionMonthly(int year, string level, string vertical);
    }
}
