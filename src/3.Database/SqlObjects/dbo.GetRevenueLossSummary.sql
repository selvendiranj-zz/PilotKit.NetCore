--DROP PROCEDURE dbo.GetRevenueLossSummary
CREATE PROCEDURE dbo.GetRevenueLossSummary
(
     @Year     INT
    ,@Vertical varchar(50) = NULL
    ,@Country  varchar(50) = NULL
)
AS
/*
==========================================================================
Author: Selvendiran Jayaraman
Creation Date: 04/26/2016
Description : This procedure will calculate Rvenue loss summary
for a particular account and region
Exec dbo.GetRevenueLossSummary '2016', 'TD', 'Canada'
==========================================================================
*/
BEGIN
	
	DECLARE @USDExRate DECIMAL(19,4)

	DECLARE @CountryCursor CURSOR
	DECLARE @varCountry VARCHAR(20)

	IF(@Vertical = 'Vertical')
	BEGIN
		CREATE TABLE #VerticalSummary
		(
		    Vertical                VARCHAR(50)
		   ,Country                 VARCHAR(50)
		   ,RevenueLossAsOfDate_USD DECIMAL(19,4)
		   ,Q1ActualLost_USD		DECIMAL(19,4)
		   ,Q2ActualLost_USD		DECIMAL(19,4)
		   ,Q3ActualLost_USD		DECIMAL(19,4)
		   ,Q4ActualLost_USD		DECIMAL(19,4)
		   ,TotalProjectedRevenueLost_USD DECIMAL(19,4)
		)
		
		INSERT #VerticalSummary
		SELECT 'TD' ,   'Canada' ,0,0,0,0,0,0 UNION
		SELECT 'TD' ,   'US'     ,0,0,0,0,0,0 UNION
		SELECT 'RBC',   'Canada' ,0,0,0,0,0,0 UNION
		SELECT 'RBC',   'US'     ,0,0,0,0,0,0 UNION
		SELECT 'RBC',   'UK'     ,0,0,0,0,0,0 UNION
		SELECT 'Scotia','Canada' ,0,0,0,0,0,0 UNION
		SELECT 'CIBC'  ,'Canada' ,0,0,0,0,0,0 UNION
		SELECT 'BMO'   ,'Canada' ,0,0,0,0,0,0 
		
		--SELECT * FROM #VerticalSummary

		BEGIN
			SET @CountryCursor = CURSOR FOR
			SELECT Country FROM dbo.Positions GROUP BY Country

			OPEN @CountryCursor 
			FETCH NEXT FROM @CountryCursor 
			INTO @varCountry

			WHILE @@FETCH_STATUS = 0
			BEGIN
				--YOUR ALGORITHM GOES HERE 
				IF(@varCountry = 'Canada') SET @USDExRate = 0.72
				IF(@varCountry = 'UK')     SET @USDExRate = 1.43
				IF(@varCountry = 'US')     SET @USDExRate = 1.00

				INSERT #VerticalSummary
				SELECT Vertical
					  ,Country
					  ,RevenueLossAsOfDate_USD = SUM(RevenueLossAsonDate)  * @USDExRate
					  ,Q1ActualLost_USD = (SUM(Jan) + SUM(Feb) + SUM(Mar)) * @USDExRate
					  ,Q2ActualLost_USD = (SUM(Apr) + SUM(May) + SUM(Jun)) * @USDExRate
					  ,Q3ActualLost_USD = (SUM(Jul) + SUM(Aug) + SUM(Sep)) * @USDExRate
					  ,Q4ActualLost_USD = (SUM(Oct) + SUM(Nov) + SUM(Dec)) * @USDExRate
					  ,TotalProjectedRevenueLost_USD = (SUM(Jan) + SUM(Feb) + SUM(Mar) + SUM(Apr) + SUM(May) + SUM(Jun) 
												+ SUM(Jul) + SUM(Aug) + SUM(Sep) + SUM(Oct) + SUM(Nov) + SUM(Dec)
												) * @USDExRate
				FROM   dbo.Positions pos
				WHERE  Country = @varCountry
				AND    LossAssessmentYear = @Year
				GROUP BY Vertical, Country

				FETCH NEXT FROM @CountryCursor 
				INTO @varCountry 
			END; 

			CLOSE @CountryCursor;
			DEALLOCATE @CountryCursor;
		END;

		SELECT Organization               = Vertical + ' ' + Country
			  ,RevenueLossAsOfDate       = SUM(RevenueLossAsOfDate_USD)
			  ,Q1		              = SUM(Q1ActualLost_USD)
		 	  ,Q2		              = SUM(Q2ActualLost_USD)
		 	  ,Q3		              = SUM(Q3ActualLost_USD)
		 	  ,Q4		              = SUM(Q4ActualLost_USD)
			  ,TotalProjectedRevenueLost = SUM(TotalProjectedRevenueLost_USD)
		FROM  #VerticalSummary
		GROUP BY Vertical, Country
		ORDER By 1
	END
     ELSE IF(@Vertical IS NULL OR @Country IS NULL)
	BEGIN
		CREATE TABLE #HorizontalSummary
		(
		    Horizontal                VARCHAR(50)
		   ,RevenueLossAsOfDate_USD   DECIMAL(19,4)
		   ,Q1ActualLost_USD		DECIMAL(19,4)
		   ,Q2ActualLost_USD		DECIMAL(19,4)
		   ,Q3ActualLost_USD		DECIMAL(19,4)
		   ,Q4ActualLost_USD		DECIMAL(19,4)
		   ,TotalProjectedRevenueLost_USD DECIMAL(19,4)
		)
		
		INSERT #HorizontalSummary
		SELECT 'AIM'  ,0,0,0,0,0,0 UNION
		SELECT 'BFS'  ,0,0,0,0,0,0 UNION
		SELECT 'BPS'  ,0,0,0,0,0,0 UNION
		SELECT 'CBC'  ,0,0,0,0,0,0 UNION
		SELECT 'CRM'  ,0,0,0,0,0,0 UNION
		SELECT 'DEP'  ,0,0,0,0,0,0 UNION
		SELECT 'IPM'  ,0,0,0,0,0,0 UNION
		SELECT 'IT IS',0,0,0,0,0,0 UNION
		SELECT 'OSP'  ,0,0,0,0,0,0 UNION
		SELECT 'PSAG' ,0,0,0,0,0,0 UNION
		SELECT 'QE&A' ,0,0,0,0,0,0 UNION
		SELECT 'TAO'  ,0,0,0,0,0,0
		
		BEGIN
			SET @CountryCursor = CURSOR FOR
			SELECT Country FROM dbo.Positions GROUP BY Country

			OPEN @CountryCursor 
			FETCH NEXT FROM @CountryCursor 
			INTO @varCountry

			WHILE @@FETCH_STATUS = 0
			BEGIN
				--YOUR ALGORITHM GOES HERE 
				IF(@varCountry = 'Canada') SET @USDExRate = 0.72
				IF(@varCountry = 'UK')     SET @USDExRate = 1.43
				IF(@varCountry = 'US')     SET @USDExRate = 1.00

				INSERT #HorizontalSummary
				SELECT Horizontal
					  ,RevenueLossAsOfDate_USD = SUM(RevenueLossAsonDate) * @USDExRate
					  ,Q1ActualLost_USD = (SUM(Jan) + SUM(Feb) + SUM(Mar)) * @USDExRate
					  ,Q2ActualLost_USD = (SUM(Apr) + SUM(May) + SUM(Jun)) * @USDExRate
					  ,Q3ActualLost_USD = (SUM(Jul) + SUM(Aug) + SUM(Sep)) * @USDExRate
					  ,Q4ActualLost_USD = (SUM(Oct) + SUM(Nov) + SUM(Dec)) * @USDExRate
					  ,TotalProjectedRevenueLost_USD = (SUM(Jan) + SUM(Feb) + SUM(Mar) + SUM(Apr) + SUM(May) + SUM(Jun) 
														+ SUM(Jul) + SUM(Aug) + SUM(Sep) + SUM(Oct) + SUM(Nov) + SUM(Dec)
														) * @USDExRate
				FROM   dbo.Positions pos
				WHERE (   (@Vertical IS NOT NULL) AND (Vertical  = @Vertical) 
						OR (@Vertical IS NULL)  AND (Vertical  = Vertical))
				AND    Country = @varCountry
				AND    LossAssessmentYear = @Year
				GROUP BY Horizontal

				FETCH NEXT FROM @CountryCursor 
				INTO @varCountry 
			END; 

			CLOSE @CountryCursor;
			DEALLOCATE @CountryCursor;
		END;

		SELECT Organization          = Horizontal
			 ,RevenueLossAsOfDate   = SUM(RevenueLossAsOfDate_USD)
			 ,Q1		              = SUM(Q1ActualLost_USD)
		 	 ,Q2		              = SUM(Q2ActualLost_USD)
		 	 ,Q3		              = SUM(Q3ActualLost_USD)
		 	 ,Q4		              = SUM(Q4ActualLost_USD)
			 ,TotalProjectedRevenueLost = SUM(TotalProjectedRevenueLost_USD)
		FROM  #HorizontalSummary
		GROUP BY Horizontal

	END
	ELSE IF (@Vertical IS NOT NULL AND @Country IS NOT NULL)
	BEGIN
		IF(@Country = 'Canada') SET @USDExRate = 0.72
		IF(@Country = 'UK')     SET @USDExRate = 1.43
		IF(@Country = 'US')     SET @USDExRate = 1.00

		CREATE TABLE #Detail
		(
			 Horizontal              VARCHAR(50)
			,RevenueLossAsOfDate_CAD DECIMAL(19,4)
			,RevenueLossAsOfDate_GBP DECIMAL(19,4)
			,RevenueLossAsOfDate_USD DECIMAL(19,4)
			,January                 DECIMAL(19,4)
			,February                DECIMAL(19,4)
			,March                   DECIMAL(19,4)
			,Q1ActualLost_CAD		DECIMAL(19,4)                    
			,Q1ActualLost_GBP		DECIMAL(19,4)
			,Q1ActualLost_USD		DECIMAL(19,4)                   
			,April				DECIMAL(19,4)                          
			,May					DECIMAL(19,4)                             
			,June				DECIMAL(19,4)                             
			,Q2ActualLost_CAD		DECIMAL(19,4)                    
			,Q2ActualLost_GBP		DECIMAL(19,4)
			,Q2ActualLost_USD		DECIMAL(19,4)
			,July				DECIMAL(19,4)                        
			,August				DECIMAL(19,4)                        
			,September			DECIMAL(19,4)                        
			,Q3ActualLost_CAD		DECIMAL(19,4)                
			,Q3ActualLost_GBP		DECIMAL(19,4)                  
			,Q3ActualLost_USD		DECIMAL(19,4)
			,October				DECIMAL(19,4)                        
			,November				DECIMAL(19,4)                        
			,December				DECIMAL(19,4)                        
			,Q4ActualLost_CAD		DECIMAL(19,4)                   
			,Q4ActualLost_GBP		DECIMAL(19,4)                  
			,Q4ActualLost_USD		DECIMAL(19,4)
		)

		INSERT #Detail
		SELECT 'AIM'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'BFS'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'BPS'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'CBC'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'CRM'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'DEP'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'IPM'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'IT IS',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'OSP'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'PSAG' ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'QE&A' ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 UNION
		SELECT 'TAO'  ,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0

		INSERT #Detail
		SELECT Horizontal
			  ,RevenueLossAsOfDate_CAD = SUM(RevenueLossAsonDate)
			  ,RevenueLossAsOfDate_GBP = SUM(RevenueLossAsonDate)
			  ,RevenueLossAsOfDate_USD = SUM(RevenueLossAsonDate)
			  ,January   = SUM(Jan)
			  ,February  = SUM(Feb)
			  ,March     = SUM(Mar)
			  ,Q1ActualLost_CAD = SUM(Jan) + SUM(Feb) + SUM(Mar)
			  ,Q1ActualLost_GBP = SUM(Jan) + SUM(Feb) + SUM(Mar)
			  ,Q1ActualLost_USD = SUM(Jan) + SUM(Feb) + SUM(Mar)
			  ,April     = SUM(Apr)
			  ,May       = SUM(May)
			  ,June      = SUM(Jun)
			  ,Q2ActualLost_CAD = SUM(Apr) + SUM(May) + SUM(Jun)
			  ,Q2ActualLost_GBP = SUM(Apr) + SUM(May) + SUM(Jun)
			  ,Q2ActualLost_USD = SUM(Apr) + SUM(May) + SUM(Jun)
			  ,July      = SUM(Jul)
			  ,August    = SUM(Aug)
			  ,September = SUM(Sep)
			  ,Q3ActualLost_CAD = SUM(Jul) + SUM(Aug) + SUM(Sep)
			  ,Q3ActualLost_GBP = SUM(Jul) + SUM(Aug) + SUM(Sep)
			  ,Q3ActualLost_USD = SUM(Jul) + SUM(Aug) + SUM(Sep)
			  ,October   = SUM(Oct)
			  ,November  = SUM(Nov)
			  ,December  = SUM(Dec)
			  ,Q4ActualLost_CAD = SUM(Oct) + SUM(Nov) + SUM(Dec)
			  ,Q4ActualLost_GBP = SUM(Oct) + SUM(Nov) + SUM(Dec)
			  ,Q4ActualLost_USD = SUM(Oct) + SUM(Nov) + SUM(Dec)
		FROM   dbo.Positions pos
		WHERE (   (@Vertical IS NOT NULL) AND (Vertical  = @Vertical) 
				OR (@Vertical IS NULL)  AND (Vertical  = Vertical))
		AND   (   (@Country IS NOT NULL)  AND (Country   = @Country) 
				OR (@Country IS NULL)   AND (Country   = Country))
		AND   LossAssessmentYear = @Year
		GROUP BY Horizontal
	    
		SELECT Organization              = Horizontal
			  ,RevenueLossAsOfDate_CAD  = SUM(RevenueLossAsOfDate_CAD)
			  ,RevenueLossAsOfDate_GBP  = SUM(RevenueLossAsOfDate_GBP)
			  ,RevenueLossAsOfDate_USD  = SUM(RevenueLossAsOfDate_USD)	* @USDExRate
			  ,January			   = SUM(January)
			  ,February			   = SUM(February)
		 	  ,March				   = SUM(March)
		 	  ,Q1ActualLost_CAD		   = SUM(Q1ActualLost_CAD)
		 	  ,Q1ActualLost_GBP		   = SUM(Q1ActualLost_GBP)
			  ,Q1ActualLost_USD		   = SUM(Q1ActualLost_USD) * @USDExRate
		 	  ,April				   = SUM(April)
		 	  ,May				   = SUM(May)
		 	  ,June				   = SUM(June)
		 	  ,Q2ActualLost_CAD		   = SUM(Q2ActualLost_CAD)
		 	  ,Q2ActualLost_GBP		   = SUM(Q2ActualLost_GBP)
			  ,Q2ActualLost_USD		   = SUM(Q2ActualLost_USD) * @USDExRate                        
		 	  ,July				   = SUM(July)
		 	  ,August				   = SUM(August)
		 	  ,September			   = SUM(September)
		 	  ,Q3ActualLost_CAD		   = SUM(Q3ActualLost_CAD)
		 	  ,Q3ActualLost_GBP		   = SUM(Q3ActualLost_GBP)
			  ,Q3ActualLost_USD		   = SUM(Q3ActualLost_USD) * @USDExRate
		 	  ,October			   = SUM(October)
		 	  ,November			   = SUM(November)
		 	  ,December			   = SUM(December)
		 	  ,Q4ActualLost_CAD		   = SUM(Q4ActualLost_CAD)
		 	  ,Q4ActualLost_GBP		   = SUM(Q4ActualLost_GBP)
			  ,Q4ActualLost_USD		   = SUM(Q4ActualLost_USD) * @USDExRate
		FROM #Detail
		GROUP BY Horizontal

	END

	--DROP TABLE #HorizontalSummary
	--DROP TABLE #Detail
END
