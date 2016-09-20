CREATE FUNCTION dbo.GetNetWorkDays
(
     @startdate datetime,
	@enddate   datetime
) 
RETURNS INT
AS
/*--=========================================================================
Author: Selvendiran Jayaraman
Create Date: 04/21/2016
Description: This procedure will calculate the number of working days
between the given dates
Select dbo.GetNetWorkDays('04/30/2016', '05/31/2016')
--=========================================================================*/
BEGIN

     declare @i     int
     declare @count int
     declare @diff  int
     
	set @i     = 0
     set @count = 0

	IF (@startdate <= @enddate)
     BEGIN
	 	 SET @diff = DATEDIFF(d, @startdate, @enddate)
	 	 
     	 WHILE(@i <= @diff)
     	 BEGIN
         	       SELECT @count = @count + 1
		  	  WHERE DATENAME(dw, DATEADD(d, @i, @startdate)) NOT IN('Saturday','Sunday')
          	  SET @i = @i + 1
     	 END     
	 END
	 
	 RETURN @count
END