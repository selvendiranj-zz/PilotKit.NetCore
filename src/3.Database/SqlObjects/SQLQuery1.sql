Select * From [dbo].[AspNetRoleClaims]
Select * From [dbo].[AspNetRoles]
Select * From [dbo].[AspNetUserClaims]
Select * From [dbo].[AspNetUserLogins]
Select * From [dbo].[AspNetUserRoles]
Select * From [dbo].[AspNetUsers]

--Truncate Table [dbo].[Positions]
Select * From [dbo].[Positions] where Vertical = 'td' and Country = 'canada'
and year(ProjectedAssignmentStartDate) = year(GETDATE())
order by ProjectedAssignmentStartDate
Select * From [dbo].[PilotKitMenu]

select year(getdate())

select	count(*)
from	F_TABLE_DATE('20160401', '20160430')
where	WEEKDAY_NAME not in ('Sat', 'Sun')

select DATEDIFF(d, '005/01/2016', '04/30/2016')

Select SONumber, RevenueLossAsonDate = dbo.GetNetWorkDays(ProjectedAssignmentStartDate, GETDATE())
                                          * CASE Location WHEN 'Offshore' THEN 9 ELSE 8 END
										  * PotentialBillRate * NumberOfRequirements
From dbo.Positions

delete from Positions Where 0=1

SELECT Organization = Horizontal, RevenueLossAsOfDate = SUM(RevenueLossAsonDate)
	 ,Q1 = SUM(Jan + Feb + Mar), Q2 = SUM(Apr + May + Jun), Q3 = SUM(Jul + Aug + Sep), Q4 = SUM(Oct + Nov + Dec)
	 ,TotalProjectedRevenueLost = SUM(Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec)
FROM   dbo.Positions pos
WHERE  Vertical = 'td'
GROUP BY Horizontal

Select distinct Horizontal from Positions where Vertical = 'td' and Country = 'us'

select sum(RevenueLossAsonDate)*0.72 from Positions
where Vertical = 'TD'
AND Horizontal = 'AIM'

