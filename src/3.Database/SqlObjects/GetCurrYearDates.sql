--DROP PROCEDURE [dbo].[GetCurrYearDates]
CREATE PROCEDURE [dbo].[GetCurrYearDates]
(
    @Year INT = NULL
)
AS
/*--=========================================================================
Author: Selvendiran Jayaraman
Create Date: 04/21/2016
Description: This procedure will return all start date and end date of 
all month in a given year
Exec GetCurrYearDates 2016
--=========================================================================*/
BEGIN
	IF @Year IS NULL SET @Year = YEAR(GETDATE())

	DECLARE @StartDate  DATETIME = '01/01/' + CONVERT(VARCHAR, @Year), --DATEADD(yy, DATEDIFF(yy,0, getdate()), 0),
		   @EndDate    DATETIME = '12/31/' + CONVERT(VARCHAR, @Year)  --DATEADD(yy, DATEDIFF(yy,0, getdate()) + 1, -1);
        
	CREATE TABLE #TempCurrYearDates([MonthName] VARCHAR(3), [Month] INT, FirstDay DATETIME, LastDay DATETIME)

	INSERT  #TempCurrYearDates
	SELECT  CONVERT(varchar(3),DATENAME(MONTH, DATEADD(MONTH, x.number, @StartDate))) AS MonthName,
	MONTH(DATEADD(dd,-(DAY(DATEADD(MONTH, x.number, @StartDate))-1),DATEADD(MONTH, x.number, @StartDate))) AS Month,
	CONVERT(VARCHAR(10), DATEADD(dd,-(DAY(DATEADD(MONTH, x.number, @StartDate))-1),DATEADD(MONTH, x.number, @StartDate)),101) AS FirstDay,
	CONVERT(VARCHAR(10), DATEADD(dd,-(DAY(DATEADD(mm,1,DATEADD(MONTH, x.number, @StartDate)))),DATEADD(mm,1,DATEADD(MONTH, x.number, @StartDate))),101) AS LastDay
	FROM    master..spt_values x
	WHERE   x.type = 'P'        
	AND     x.number <= DATEDIFF(MONTH, @StartDate, @EndDate)

	SELECT * FROM #TempCurrYearDates
END