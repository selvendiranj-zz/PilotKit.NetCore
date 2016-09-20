--DROP PROCEDURE dbo.GetPositionSummary
CREATE PROCEDURE dbo.GetPositionSummary
(
     @Year     INT
    ,@Level    varchar(20)
    ,@Vertical varchar(20) = NULL
)
AS
/*--=========================================================================
Author: Selvendiran Jayaraman
Create Date: 04/28/2016
Description: This procedure will return horizonatal total postions as well as
account level total positions that are open
dbo.GetPositionSummary '2015', 'horizontal', 'BFS'
--=========================================================================*/
BEGIN
	 
	 CREATE TABLE #OpenPositions
	 (
	    [Level]           VARCHAR(20)
	   ,OnsitePositions   INT
	   ,OffshorePositions INT
	   ,TotalPositions    INT
	   ,LostPositions     INT
	 )
	 CREATE TABLE #OPDetail([Level] VARCHAR(20), Onsite INT, Offshore INT)
	 CREATE TABLE #LostPositions([Level] VARCHAR(20), LostPositions INT)
	 
	 IF(@Level = 'Horizontal')
	 BEGIN
	        INSERT #OpenPositions
			SELECT 'AIM'  ,0,0,0,0 UNION
			SELECT 'BFS'  ,0,0,0,0 UNION
			SELECT 'BPS'  ,0,0,0,0 UNION
			SELECT 'CBC'  ,0,0,0,0 UNION
			SELECT 'CRM'  ,0,0,0,0 UNION
			SELECT 'DEP'  ,0,0,0,0 UNION
			SELECT 'IPM'  ,0,0,0,0 UNION
			SELECT 'IT IS',0,0,0,0 UNION
			SELECT 'OSP'  ,0,0,0,0 UNION
			SELECT 'PSAG' ,0,0,0,0 UNION
			SELECT 'QE&A' ,0,0,0,0 UNION
			SELECT 'TAO'  ,0,0,0,0

			INSERT #OPDetail
		     SELECT *
			FROM (
				SELECT Horizontal [Level]
					  ,NumberOfRequirements
					  ,Location
				FROM   dbo.Positions
				WHERE ((@Vertical IS NOT NULL) AND (Vertical  = @Vertical) 
						OR (@Vertical IS NULL) AND (Vertical  = Vertical))
				AND    YEAR(ProjectedAssignmentStartDate) = @Year
			) as src
			PIVOT
			(
				SUM(NumberOfRequirements)
				FOR Location IN ([Onsite], [Offshore])
			)AS pvt

			
			INSERT #LostPositions
			SELECT Horizontal [Level]
			      ,Sum(NumberOfPositionLost) [LostPositions]
			FROM   dbo.Positions
			WHERE ((@Vertical IS NOT NULL) AND (Vertical  = @Vertical) 
				OR (@Vertical IS NULL) AND (Vertical  = Vertical))
			AND    YEAR(ProjectedAssignmentStartDate) = @Year
			GROUP BY Horizontal

			--SELECT * FROM #OPDetail
			--SELECT * FROM #LostPositions

			INSERT #OpenPositions
			SELECT op.Level                
			      ,ISNULL(op.Onsite, 0) [Onsite]
				  ,ISNULL(op.Offshore, 0) [Offshore]
				  ,(ISNULL(op.Onsite, 0) + ISNULL(op.Offshore, 0)) [TotalPositions]
				  ,lp.LostPositions        
			FROM   #OPDetail     op
			LEFt JOIN #LostPositions lp ON op.Level = lp.Level

			--SELECT * FROM #OpenPositions

			SELECT [Level]
			      ,SUM(OnsitePositions)   [OnsitePositions]
				 ,SUM(OffshorePositions) [OffshorePositions]
				 ,SUM(TotalPositions)    [TotalPositions]
				 ,SUM(LostPositions)     [LostPositions]
			INTO   #TempHorzPos
			FROM   #OpenPositions
			GROUP BY Level

			SELECT [Level]
			      ,OnsitePositions   [OnsitePositions]
				 ,OffshorePositions [OffshorePositions]
				 ,(TotalPositions - LostPositions)   [TotalPositions]
				 ,LostPositions     [LostPositions]
			FROM   #TempHorzPos
	 END
	 ELSE IF(@Level = 'Vertical')
	 BEGIN
	        INSERT #OpenPositions
			SELECT 'TD Canada'     ,0,0,0,0 UNION
			SELECT 'TD US'         ,0,0,0,0 UNION
			SELECT 'RBC Canada'    ,0,0,0,0 UNION
			SELECT 'RBC US'        ,0,0,0,0 UNION
			SELECT 'RBC UK'        ,0,0,0,0 UNION
			SELECT 'Scotia Canada' ,0,0,0,0 UNION
			SELECT 'CIBC Canada'   ,0,0,0,0 UNION
			SELECT 'BMO Canada'    ,0,0,0,0 

			INSERT #OPDetail
		     SELECT *
			FROM (
				SELECT Vertical + ' ' + Country [Level]
					  ,NumberOfRequirements
					  ,Location
				FROM   dbo.Positions
				WHERE  YEAR(ProjectedAssignmentStartDate) = @Year
			) as src
			PIVOT
			(
				SUM(NumberOfRequirements)
				FOR Location IN ([Onsite], [Offshore])
			)AS pvt

			
			INSERT #LostPositions
			SELECT Vertical + ' ' + Country [Level]
			      ,SUM(NumberOfPositionLost) [LostPositions]
			FROM   dbo.Positions
			WHERE  YEAR(ProjectedAssignmentStartDate) = @Year
			GROUP BY Vertical, Country

			--SELECT * FROM #OPDetail
			--SELECT * FROM #LostPositions

			INSERT #OpenPositions
			SELECT op.Level                
			      ,ISNULL(op.Onsite, 0) [Onsite]
				 ,ISNULL(op.Offshore, 0) [Offshore]
				 ,(ISNULL(op.Onsite, 0) + ISNULL(op.Offshore, 0)) [TotalPositions]
				 ,lp.LostPositions        
			FROM   #OPDetail     op
			LEFt JOIN #LostPositions lp ON op.Level = lp.Level

			--SELECT * FROM #OpenPositions

			SELECT [Level]
			      ,SUM(OnsitePositions)   [OnsitePositions]
				  ,SUM(OffshorePositions) [OffshorePositions]
				  ,SUM(TotalPositions)    [TotalPositions]
				  ,SUM(LostPositions)     [LostPositions]
			INTO   #TempVertPos
			FROM   #OpenPositions
			GROUP BY Level

			SELECT [Level]
			      ,OnsitePositions   [OnsitePositions]
				 ,OffshorePositions [OffshorePositions]
				 ,(TotalPositions - LostPositions)    [TotalPositions]
				 ,LostPositions     [LostPositions]
			FROM   #TempVertPos
			
			--DROP TABLE #OPDetail
			--DROP TABLE #LostPositions
	 END

	 --DROP TABLE #OpenPositions
END
