--DROP PROCEDURE [dbo].[CalculateRevenueLoss]
CREATE PROCEDURE [dbo].[CalculateRevenueLoss]
(
    @Year INT
)
AS
/*--=========================================================================
Author: Selvendiran Jayaraman
Create Date: 04/21/2016
Description: This procedure will calculate revenue loss for all individual
month as well as revenueloss as of date
Exec [dbo].[CalculateRevenueLoss] 2016
--=========================================================================*/
BEGIN

		CREATE TABLE #TempCurrYearDates([MonthName] VARCHAR(3), [Month] INT, FirstDay DATETIME, LastDay DATETIME)

		INSERT  #TempCurrYearDates
		Exec GetCurrYearDates @Year
		
		--Select * From #TempCurrYearDates
		
	    UPDATE pos
      --Select
	    Set    
	         --SONumber,
			 --ProjectedAssignmentStartDate,
		       RevenueLossAsonDate = dbo.GetNetWorkDays(ProjectedAssignmentStartDate, GETDATE()) * 
											CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END * 
			     								PotentialBillRate * NumberOfRequirements
			  ,Jan = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Jan')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jan')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Jan'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Jan'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Jan'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Feb = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Feb')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Feb')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Feb'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Feb'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Feb'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Mar = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Mar')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Mar')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Mar'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Mar'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Mar'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Apr = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Apr')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Apr')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Apr'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Apr'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Apr'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,May = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'May')
						  THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'May')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'May'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'May'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
						  ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'May'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Jun = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Jun')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jun')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Jun'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Jun'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Jun'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Jul = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Jul')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jul')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Jul'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Jul'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Jul'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Aug = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Aug')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Aug')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Aug'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Aug'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Aug'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Sep = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Sep')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Sep')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Sep'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Sep'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Sep'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Oct = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Oct')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Oct')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Oct'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Oct'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Oct'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,Nov = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Nov')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Nov')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Nov'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Nov'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Nov'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
			  ,[Dec] = CASE WHEN ProjectedAssignmentStartDate > (Select [LastDay] From #TempCurrYearDates Where MonthName = 'Dec')
			              THEN 0.00
						  WHEN ProjectedAssignmentStartDate < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Dec')
						  THEN dbo.GetNetWorkDays((Select FirstDay From #TempCurrYearDates Where MonthName = 'Dec'),
						                          (Select LastDay From #TempCurrYearDates Where MonthName = 'Dec'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					      ELSE dbo.GetNetWorkDays(ProjectedAssignmentStartDate, (Select LastDay From #TempCurrYearDates Where MonthName = 'Dec'))
								          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
			     						  * PotentialBillRate * NumberOfRequirements
					 END
		
		From   dbo.Positions pos
          Where  pos.LossAssessmentYear = @Year

	    
		Update pos
		Set    Jan = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jan') THEN 0.00 ELSE Jan END
		      ,Feb = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Feb') THEN 0.00 ELSE Feb END
			  ,Mar = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Mar') THEN 0.00 ELSE Mar END
			  ,Apr = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Apr') THEN 0.00 ELSE Apr END
			  ,May = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'May') THEN 0.00 ELSE May END
			  ,Jun = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jun') THEN 0.00 ELSE Jun END
			  ,Jul = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jul') THEN 0.00 ELSE Jul END
			  ,Aug = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Aug') THEN 0.00 ELSE Aug END
			  ,Sep = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Sep') THEN 0.00 ELSE Sep END
			  ,Oct = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Oct') THEN 0.00 ELSE Oct END
			  ,Nov = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Nov') THEN 0.00 ELSE Nov END
			  ,Dec = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Dec') THEN 0.00 ELSE Dec END
		From   dbo.Positions pos
          Where  pos.LossAssessmentYear = @Year

		Drop Table #TempCurrYearDates
		
      --SELECT * FROM Positions

END
