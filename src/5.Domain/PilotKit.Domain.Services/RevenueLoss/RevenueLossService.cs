using PilotKit.Domain.Interfaces.RevenueLoss;
using PilotKit.Domain.Model.RevenueLoss;
using PilotKit.Domain.Services.Common;
using PilotKit.Infrastructure.CrossCutting.Constants;
using PilotKit.Infrastructure.CrossCutting.Extensions;
using PilotKit.Repository.Interfaces.RevenueLoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Services.RevenueLoss
{
    public class RevenueLossService : DomainServiceBase, IRevenueLossService
    {
        private IRevenueLossRepository repository;

        public RevenueLossService(IRevenueLossRepository repository)
        {
            this.repository = repository;
        }

        public IList<PositionMonthly> GetPositionMonthly(string level, string vertical)
        {
            if (vertical == null)
            {
                return this.repository.GetPositionMonthly(RevLossConstants.CurrentAssesmentYear, level);
            }
            else
            {
                return this.repository.GetPositionMonthly(RevLossConstants.CurrentAssesmentYear, level, vertical);
            }
        }

        public IList<Positions> GetPositions()
        {
            return this.repository.GetPositions();
        }

        public IList<PositionSummary> GetPositionSummary(string level, string vertical)
        {
            if (vertical == null)
            {
                return this.repository.GetPositionSummary(RevLossConstants.CurrentAssesmentYear, level);
            }
            else
            {
                return this.repository.GetPositionSummary(RevLossConstants.CurrentAssesmentYear, level, vertical);
            }
        }

        public IList<Positions> GetRevenueLossDetail(string vertical, string country)
        {
            if (country == null)
            {
                return this.repository.GetRevenueLossDetail(vertical);
            }
            else
            {
                return this.repository.GetRevenueLossDetail(vertical, country);
            }
        }

        public IList<Summary> GetRevenueLossSummary(string vertical)
        {
            if (vertical == null)
            {
                return this.repository.GetRevenueLossSummary(RevLossConstants.CurrentAssesmentYear);
            }
            else
            {
                return this.repository.GetRevenueLossSummary(RevLossConstants.CurrentAssesmentYear, vertical);
            }
        }

        public IList<SummaryDetail> GetRevenueLossSummaryDetail(string vertical, string country)
        {
            return this.repository.GetRevenueLossSummaryDetail(RevLossConstants.CurrentAssesmentYear, vertical, country);
        }

        public void RemoveAllPositions()
        {
            this.repository.RemoveAllPositions(RevLossConstants.CurrentAssesmentYear);
        }

        public void UploadPositions(IList<Positions> positions, string sheetName)
        {
            positions.ForEach((p) =>
            {
                if (sheetName.Contains('|'))
                {
                    p.Vertical = sheetName.Split('|')[0].Trim();
                    p.Country = sheetName.Split('|')[1].Trim();
                }
                else
                {
                    p.Vertical = sheetName.Trim();
                    p.Country = null;
                }
                p.AuditID = "";
                p.CreationDate = DateTime.Now;
                p.LossAssessmentYear = RevLossConstants.CurrentAssesmentYear;
            });

            this.repository.InsertPositions(positions);
            this.repository.CalculateRevenueLoss(RevLossConstants.CurrentAssesmentYear);
        }
    }
}
