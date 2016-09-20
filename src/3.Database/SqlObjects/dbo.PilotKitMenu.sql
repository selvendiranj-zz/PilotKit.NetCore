USE [PilotKit]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--DROP TABLE [dbo].[PilotKitMenu]
CREATE TABLE [dbo].[PilotKitMenu]
(
      [ID]          BIGINT        NOT NULL PRIMARY KEY
     ,[Name]        VARCHAR (100) NOT NULL
     ,[Description] VARCHAR (100) NOT NULL
	,[IconName]    VARCHAR (100) NULL
     ,[ParentId]    BIGINT        NULL
     ,[RelativeUrl] VARCHAR (100) NULL
	,[UrlParams]   VARCHAR (100) NULL
     ,[Role]        VARCHAR (100) NULL
     ,[Category]    VARCHAR (100) NOT NULL
);

GO

--SELECT * FROM [dbo].[PilotKitMenu]
--UPDATE [dbo].[PilotKitMenu] set RelativeUrl = NULL

--TRUNCATE TABLE dbo.PilotKitMenu

INSERT dbo.PilotKitMenu
SELECT	1  , 'Dashboard' ,'Dashboard' ,'fa fa-dashboard fa-fw', 0 , '/revenueloss/dashboard', NULL          , NULL, 'RevenueLoss' UNION
SELECT	2  , 'Positions' ,'Positions' ,'fa fa-users fa-fw'    , 0 , '/revenueloss/positions', NULL          , NULL, 'RevenueLoss' UNION
SELECT	3  , 'Summary'   ,'Summary'   ,'fa fa-bar-chart fa-fw', 0 , '#'	                   , NULL          , NULL, 'RevenueLoss' UNION
SELECT	4  , 'Detail'    ,'Detail'    ,'fa fa-book fa-fw'     , 0 , '#'	                   , NULL          , NULL, 'RevenueLoss' UNION
SELECT	5  , 'Upload'    ,'Upload'    ,'fa fa-upload fa-fw'   , 0 , '/revenueloss/upload'   , NULL          , NULL, 'RevenueLoss' UNION
SELECT	6  , 'Export'    ,'Export'    ,'glyphicon glyphicon-export fa-fw', 0 , '/revenueloss/export'   , NULL          , NULL, 'RevenueLoss' UNION
SELECT	7  , 'TD Bank'   ,'TD Bank'   ,''                     , 3 ,	'/revenueloss/summary'  , '/td'         , NULL, 'RevenueLoss' UNION
SELECT	8  , 'RBC'       ,'RBC'       ,''                     , 3 ,	'/revenueloss/summary'  , '/rbc'        , NULL, 'RevenueLoss' UNION
SELECT	9  , 'Scotia'    ,'Scotia'    ,''                     , 3 ,	'/revenueloss/summary'  , '/scotia'     , NULL, 'RevenueLoss' UNION
SELECT	10 , 'CIBC'      ,'CIBC'      ,''                     , 3 ,	'/revenueloss/summary'  , '/cibc'       , NULL, 'RevenueLoss' UNION
SELECT	11 , 'BMO'       ,'BMO'       ,''                     , 3 ,	'/revenueloss/summary'  , '/bmo'        , NULL, 'RevenueLoss' UNION
SELECT	12 , 'TD Canada' ,'TD Canada' ,''                     , 4 ,	'/revenueloss/detail'   , '/td/canada'  , NULL, 'RevenueLoss' UNION
SELECT	13 , 'TD US'     ,'TD US'     ,''                     , 4 ,	'/revenueloss/detail'   , '/td/us'      , NULL, 'RevenueLoss' UNION
SELECT	14 , 'RBC Canada','RBC Canada',''                     , 4 ,	'/revenueloss/detail'   , '/rbc/canada' , NULL, 'RevenueLoss' UNION
SELECT	15 , 'RBC US'    ,'RBC US'    ,''                     , 4 ,	'/revenueloss/detail'   , '/rbc/us'     , NULL, 'RevenueLoss' UNION
SELECT	16 , 'RBC UK'    ,'RBC UK'    ,''                     , 4 ,	'/revenueloss/detail'   , '/rbc/uk'     , NULL, 'RevenueLoss' UNION
SELECT	17 , 'Scotia'    ,'Scotia'    ,''                     , 4 ,	'/revenueloss/detail'   , '/scotia/null', NULL, 'RevenueLoss' UNION
SELECT	18 , 'CIBC'      ,'CIBC'      ,''                     , 4 ,	'/revenueloss/detail'   , '/cibc/null'  , NULL, 'RevenueLoss' UNION
SELECT	19 , 'BMO'       ,'BMO'       ,''                     , 4 ,	'/revenueloss/detail'   , '/bmo/null'   , NULL, 'RevenueLoss' 
