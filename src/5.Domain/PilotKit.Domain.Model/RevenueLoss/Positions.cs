using PilotKit.Infrastructure.Interfaces.ExcelImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Model.RevenueLoss
{
    public class Positions : ExcelModelBase
    {
        public long ID { get; set; }
        public string AuditID { get; set; }
        public DateTime CreationDate { get; set; }
        public long SONumber { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ProjectedAssignmentStartDate { get; set; }
        public string Vertical { get; set; }
        public string Horizontal { get; set; }
        public string Country { get; set; }
        public string ProjectName { get; set; }
        public string Role { get; set; }
        public string SkillSet { get; set; }
        public string AssociateDesignation { get; set; }
        public string TypeOfBilling { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Comments { get; set; }
        public string SourceOfDemand { get; set; }
        public int LossAssessmentYear { get; set; }
        public int DurationOfAssignment { get; set; }
        public int NumberOfRequirements { get; set; }
        public int NumberOfPositionLost { get; set; }
        public int YearsOfExperience { get; set; }
        public int Age { get; set; }
        public bool IsReplacement { get; set; }
        public bool IsCritical { get; set; }
        public bool HasClientInterview { get; set; }
        public bool IsMarkedForHiring { get; set; }
        public bool IsSOCreated { get; set; }
        public double PotentialBillRate { get; set; }
        public double RevenueLossAsonDate { get; set; }
        public double Jan { get; set; }
        public double Feb { get; set; }
        public double Mar { get; set; }
        public double Apr { get; set; }
        public double May { get; set; }
        public double Jun { get; set; }
        public double Jul { get; set; }
        public double Aug { get; set; }
        public double Sep { get; set; }
        public double Oct { get; set; }
        public double Nov { get; set; }
        public double Dec { get; set; }
        public double MonthlyRevenue { get; set; }
        public double Confidence_WithinMonth { get; set; }
        public double Confidence_Within3Month { get; set; }
        public double Confidence_Projected { get; set; }
    }
}