--DROP TABLE [dbo].[Positions]
CREATE TABLE [dbo].[Positions]
(
      [ID]                           BIGINT        NOT NULL IDENTITY(1,1)
	,[AuditID]                      VARCHAR(10)   NOT NULL
	,[CreationDate]                 DATETIME      NOT NULL -- Record inserted date
   --,[IsActive]                     BIT           NOT NULL -- Maintain history
	,[SONumber]                     BIGINT        NOT NULL
     ,[RequestDate]                  DATE          NOT NULL
	,[ProjectedAssignmentStartDate] DATE          NOT NULL
	,[Vertical]                     VARCHAR(100)  NOT NULL
	,[Horizontal]                   VARCHAR(100)  NOT NULL
	,[Country]                      VARCHAR(100)  NOT NULL
     ,[ProjectName]                  VARCHAR(100)  NOT NULL
     ,[Role]	                      VARCHAR(100)  NOT NULL
     ,[SkillSet]                     VARCHAR(1000) NOT NULL
     ,[AssociateDesignation]         VARCHAR(3)        NULL
     ,[TypeOfBilling]                VARCHAR(20)       NULL -- FixedPrice/T&M
	,[Location]                     VARCHAR(100)  NOT NULL -- Onsite/Offshore
     ,[City]                         VARCHAR(100)  NOT NULL
     ,[Comments]                     VARCHAR(500)      NULL
	,[SourceOfDemand]               VARCHAR(100)      NULL -- SignedDeal / KnownDeal / UnknownStaffAug
	,[LossAssessmentYear]           INT           NOT NULL
     ,[DurationOfAssignment]         INT               NULL -- (in Months)
	,[NumberOfRequirements]         INT           NOT NULL
     ,[NumberOfPositionLost]         INT               NULL
	,[YearsOfExperience]            INT               NULL
	,[Age]                          INT           NOT NULL
     ,[IsReplacement]                BIT           NOT NULL -- Y/N
     ,[IsCritical]                   BIT           NOT NULL -- Y/N
     ,[HasClientInterview]           BIT           NOT NULL -- Y/N
	,[IsMarkedForHiring]            BIT               NULL
	,[IsSOCreated]                  BIT               NULL
	,[PotentialBillRate]            DECIMAL(19,4) NOT NULL
     ,[RevenueLossAsonDate]          DECIMAL(19,4)     NULL
     ,[Jan]                          DECIMAL(19,4)     NULL
     ,[Feb]                          DECIMAL(19,4)     NULL
     ,[Mar]                          DECIMAL(19,4)     NULL
     ,[Apr]                          DECIMAL(19,4)     NULL
     ,[May]                          DECIMAL(19,4)     NULL
     ,[Jun]                          DECIMAL(19,4)     NULL
     ,[Jul]                          DECIMAL(19,4)     NULL
     ,[Aug]                          DECIMAL(19,4)     NULL
     ,[Sep]                          DECIMAL(19,4)     NULL
     ,[Oct]                          DECIMAL(19,4)     NULL
     ,[Nov]                          DECIMAL(19,4)     NULL
     ,[Dec]                          DECIMAL(19,4)     NULL
	,[MonthlyRevenue]               DECIMAL(19,4)     NULL
	,[Confidence_WithinMonth]       DECIMAL(3,2)      NULL -- Percentage
	,[Confidence_Within3Month]      DECIMAL(3,2)      NULL -- Percentage
	,[Confidence_Projected]         DECIMAL(3,2)      NULL -- Percentage
	,CONSTRAINT pk_SONumber_LossAssessmentYear PRIMARY KEY (ID, SONumber, LossAssessmentYear)
)

--TRUNCATE TABLE [dbo].[Positions]
--SELECT * FROM [dbo].[Positions] Where Vertical = 'scotia' and Country = 'Canada'