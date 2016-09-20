--DROP PROCEDURE dbo.GetPositionMonthly
CREATE PROCEDURE dbo.GetPositionMonthly
(
	 @Year INT
	,@Level VARCHAR(20)
	,@Vertical VARCHAR(20) = NULL
)
AS
/*--=========================================================================
Author: Selvendiran Jayaraman
Create Date: 04/28/2016
Description: This procedure will return horizonatal monthly cumulative postions
as well as account level monthly cumulative positions that are open
dbo.GetPositionMonthly '2016', 'Horizontal', 'td' --'Horizontal', 'Vertical'
--=========================================================================*/
BEGIN
	 
      CREATE TABLE #TempCurrYearDates([MonthName] VARCHAR(3), [Month] INT, FirstDay DATETIME, LastDay DATETIME)

	 INSERT  #TempCurrYearDates
	 Exec GetCurrYearDates @Year

	 CREATE TABLE #OpenPositions
	 (
	    [Level]           VARCHAR(20)
	   ,Jan               INT
	   ,Feb               INT
	   ,Mar               INT
	   ,Apr               INT
	   ,May               INT
	   ,Jun               INT
	   ,Jul               INT
	   ,Aug               INT
	   ,Sep               INT
	   ,Oct               INT
	   ,Nov               INT
	   ,Dec               INT
	 )

	 SELECT * INTO #TempPositions FROM #OpenPositions
	 
	 IF(@Level = 'Horizontal')
	 BEGIN
	        INSERT #OpenPositions
			SELECT 'AIM'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'BFS'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'BPS'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'CBC'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'CRM'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'DEP'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'IPM'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'IT IS',0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'OSP'  ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'PSAG' ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'QE&A' ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
			SELECT 'TAO'  ,0,0,0,0,0,0,0,0,0,0,0,0

		    INSERT #OpenPositions
		    SELECT
				Horizontal [Level],
				[1]  AS Jan,
				[2]  AS Feb,
				[3]  AS Mar,
				[4]  AS Apr,
				[5]  AS May,
				[6]  AS Jun,
				[7]  AS Jul,
				[8]  AS Aug,
				[9]  AS Sep,
				[10] AS Oct,
				[11] AS Nov,
				[12] AS Dec
			FROM (
				SELECT 
					Horizontal,
					NumberOfRequirements,
					MONTH(ProjectedAssignmentStartDate) as TMonth
				FROM dbo.Positions
				WHERE ((@Vertical IS NOT NULL) AND (Vertical  = @Vertical) 
				OR     (@Vertical IS NULL)     AND (Vertical  = Vertical))
				AND YEAR(ProjectedAssignmentStartDate) = @Year) src  
			PIVOT
			(
				SUM(NumberOfRequirements)
				FOR TMonth
				IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
			) AS pvtMonth

			--SELECT * FROM #OpenPositions
			
			INSERT #TempPositions
			SELECT [Level]
			      ,Jan = SUM(Jan)
				 ,Feb = SUM(Feb)
				 ,Mar = SUM(Mar)
				 ,Apr = SUM(Apr)
				 ,May = SUM(May)
				 ,Jun = SUM(Jun)
				 ,Jul = SUM(Jul)
				 ,Aug = SUM(Aug)
				 ,Sep = SUM(Sep)
				 ,Oct = SUM(Oct)
				 ,Nov = SUM(Nov)
				 ,Dec = SUM(Dec)
			FROM  #OpenPositions
			GROUP BY Level

	 END
	 ELSE IF(@Level = 'Vertical')
	 BEGIN
		INSERT #OpenPositions
		SELECT 'TD Canada'     ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'TD US'         ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'RBC Canada'    ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'RBC US'        ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'RBC UK'        ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'Scotia Canada' ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'CIBC Canada'   ,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'BMO Canada'    ,0,0,0,0,0,0,0,0,0,0,0,0
		
		INSERT #OpenPositions
		SELECT
			[Level],
			[1]  AS Jan,
			[2]  AS Feb,
			[3]  AS Mar,
			[4]  AS Apr,
			[5]  AS May,
			[6]  AS Jun,
			[7]  AS Jul,
			[8]  AS Aug,
			[9]  AS Sep,
			[10] AS Oct,
			[11] AS Nov,
			[12] AS Dec
		FROM (
			SELECT 
				Vertical + ' ' + Country [Level],
				NumberOfRequirements,
				MONTH(ProjectedAssignmentStartDate) as TMonth
			FROM dbo.Positions
			WHERE YEAR(ProjectedAssignmentStartDate) = @Year) src
			PIVOT
			(
				SUM(NumberOfRequirements)
				FOR TMonth
				IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
			) AS pvtMonth

			--SELECT * FROM #OpenPositions
			
		INSERT #TempPositions
		SELECT [Level]
		      ,Jan = SUM(Jan)
			 ,Feb = SUM(Feb)
			 ,Mar = SUM(Mar)
			 ,Apr = SUM(Apr)
			 ,May = SUM(May)
			 ,Jun = SUM(Jun)
			 ,Jul = SUM(Jul)
			 ,Aug = SUM(Aug)
			 ,Sep = SUM(Sep)
			 ,Oct = SUM(Oct)
			 ,Nov = SUM(Nov)
			 ,Dec = SUM(Dec)
		FROM  #OpenPositions
		GROUP BY Level
	 END

	 UPDATE #TempPositions
	 SET    Jan = Jan
		  ,Feb = Jan + Feb
		  ,Mar = Jan + Feb + Mar
		  ,Apr = Jan + Feb + Mar + Apr
		  ,May = Jan + Feb + Mar + Apr + May
		  ,Jun = Jan + Feb + Mar + Apr + May + Jun
		  ,Jul = Jan + Feb + Mar + Apr + May + Jun + Jul
		  ,Aug = Jan + Feb + Mar + Apr + May + Jun + Jul + Aug
		  ,Sep = Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep
		  ,Oct = Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct
		  ,Nov = Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov
		  ,Dec = Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec
	 
	 UPDATE pos
	 SET    Jan = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jan') THEN 0 ELSE Jan END
		  ,Feb = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Feb') THEN 0 ELSE Feb END
		  ,Mar = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Mar') THEN 0 ELSE Mar END
		  ,Apr = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Apr') THEN 0 ELSE Apr END
		  ,May = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'May') THEN 0 ELSE May END
		  ,Jun = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jun') THEN 0 ELSE Jun END
		  ,Jul = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Jul') THEN 0 ELSE Jul END
		  ,Aug = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Aug') THEN 0 ELSE Aug END
		  ,Sep = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Sep') THEN 0 ELSE Sep END
		  ,Oct = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Oct') THEN 0 ELSE Oct END
		  ,Nov = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Nov') THEN 0 ELSE Nov END
		  ,Dec = CASE WHEN GETDATE() < (Select [FirstDay] From #TempCurrYearDates Where MonthName = 'Dec') THEN 0 ELSE Dec END
	 FROM  #TempPositions pos
	 
	 SELECT [Level]
		  ,Jan
		  ,Feb
		  ,Mar
		  ,Apr
		  ,May
		  ,Jun
		  ,Jul
		  ,Aug
		  ,Sep
		  ,Oct
		  ,Nov
		  ,Dec
	 FROM  #TempPositions
	 
	 --DROP TABLE #OpenPositions
END
