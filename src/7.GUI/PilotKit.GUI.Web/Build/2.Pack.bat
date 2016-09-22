SET version=1.0.0
SET appBasePath=D:\Workspace\PilotKit\src
SET dnxToolPath=%userprofile%\.dnx\runtimes\dnx-clr-win-x86.1.0.0-rc1-update2\bin\lib\Microsoft.Dnx.Tooling
SET dnxToolName=%dnxToolPath%\Microsoft.Dnx.Tooling.dll
SET srcFolder=D:\Workspace\PilotKit\src\GUI\PilotKit.GUI.Web
SET dstFolder=D:\Workspace\PilotKit\artifacts\bin
SET dstFolder=%userprofile%\.dnx\packages
SET pubFolder=D:\Workspace\PilotKit\artifacts\bin\PilotKit.GUI.Web\approot\packages\PilotKit.GUI.Web\1.0.0\root

REM GOTO STEP2

dnx --appbase "%appBasePath%\Infrastructure\PilotKit.Infrastructure.Interfaces"   "%dnxToolName%" pack "%appBasePath%\Infrastructure\PilotKit.Infrastructure.Interfaces"   --configuration lib --out "%dstFolder%\PilotKit.Infrastructure.Interfaces\%version%"
dnx --appbase "%appBasePath%\Domain\PilotKit.Domain.Model"                        "%dnxToolName%" pack "%appBasePath%\Domain\PilotKit.Domain.Model"                        --configuration lib --out "%dstFolder%\PilotKit.Domain.Model\%version%"
dnx --appbase "%appBasePath%\Infrastructure\PilotKit.Infrastructure.CrossCutting" "%dnxToolName%" pack "%appBasePath%\Infrastructure\PilotKit.Infrastructure.CrossCutting" --configuration lib --out "%dstFolder%\PilotKit.Infrastructure.CrossCutting\%version%"
dnx --appbase "%appBasePath%\Orchestration\PilotKit.Orchestration.Interfaces"     "%dnxToolName%" pack "%appBasePath%\Orchestration\PilotKit.Orchestration.Interfaces"     --configuration lib --out "%dstFolder%\PilotKit.Orchestration.Interfaces\%version%"
dnx --appbase "%appBasePath%\Repository\PilotKit.Repository.Interfaces"           "%dnxToolName%" pack "%appBasePath%\Repository\PilotKit.Repository.Interfaces"           --configuration lib --out "%dstFolder%\PilotKit.Repository.Interfaces\%version%"
dnx --appbase "%appBasePath%\Domain\PilotKit.Domain.Interfaces"                   "%dnxToolName%" pack "%appBasePath%\Domain\PilotKit.Domain.Interfaces"                   --configuration lib --out "%dstFolder%\PilotKit.Domain.Interfaces\%version%"
dnx --appbase "%appBasePath%\Repository\PilotKit.Repository.DataAccess"           "%dnxToolName%" pack "%appBasePath%\Repository\PilotKit.Repository.DataAccess"           --configuration lib --out "%dstFolder%\PilotKit.Repository.DataAccess\%version%"
dnx --appbase "%appBasePath%\Domain\PilotKit.Domain.Services"                     "%dnxToolName%" pack "%appBasePath%\Domain\PilotKit.Domain.Services"                     --configuration lib --out "%dstFolder%\PilotKit.Domain.Services\%version%"
dnx --appbase "%appBasePath%\Orchestration\PilotKit.Orchestration.Concrete"       "%dnxToolName%" pack "%appBasePath%\Orchestration\PilotKit.Orchestration.Concrete"       --configuration lib --out "%dstFolder%\PilotKit.Orchestration.Concrete\%version%"
dnx --appbase "%appBasePath%\GUI\PilotKit.GUI.Web"                                "%dnxToolName%" pack "%appBasePath%\GUI\PilotKit.GUI.Web"                                --configuration lib --out "%dstFolder%\PilotKit.GUI.Web\%version%"

IF EXIST "%dstFolder%\PilotKit.GUI.Web\%version%\app" RMDIR "%dstFolder%\PilotKit.GUI.Web\%version%\app"
MOVE "%dstFolder%\PilotKit.GUI.Web\%version%\lib\app" "%dstFolder%\PilotKit.GUI.Web\%version%"


XCOPY /E /V /J /D /Y /I /Q "%srcFolder%\wwwroot" "%dstFolder%\PilotKit.GUI.Web\%version%\root\wwwroot"
XCOPY /E /V /J /D /Y /I /Q "%srcFolder%\Views"   "%dstFolder%\PilotKit.GUI.Web\%version%\root\Views"

:STEP2
MKDIR "%dstFolder%\PilotKit.GUI.Web\%version%\root\Properties\"
COPY /y "%srcFolder%\Properties"\launchSettings.json "%dstFolder%\PilotKit.GUI.Web\%version%\root\Properties"
COPY /y "%srcFolder%"\*.json "%dstFolder%\PilotKit.GUI.Web\%version%\root"

COPY /y %pubFolder%\project.json "%dstFolder%\PilotKit.GUI.Web\%version%\root"
COPY /y %pubFolder%\project.lock.json "%dstFolder%\PilotKit.GUI.Web\%version%\root"

IF NOT EXIST "%dstFolder%\PilotKit.GUI.Web\Stage" MKDIR "%dstFolder%\PilotKit.GUI.Web\Stage"
IF NOT EXIST "%dstFolder%\PilotKit.GUI.Web\Reports" MKDIR "%dstFolder%\PilotKit.GUI.Web\Reports"

@ECHO off 
SETLOCAL enableextensions disabledelayedexpansion

SET "homedir=%USERPROFILE:\=\\%"
SET searchtxt=D:\\Workspace\\PilotKit\\artifacts\\bin
SET replacetxt=%homedir%\\.dnx\\packages
SET textFile=%dstFolder%\PilotKit.GUI.Web\%version%\root\config.json

FOR /f "delims=" %%i in ('TYPE "%textFile%" ^& BREAK ^> "%textFile%"') do (
    SET "line=%%i"
    SETLOCAL enabledelayedexpansion
    SET "line=!line:%searchtxt%=%replacetxt%!"
    >> "%textFile%" ECHO(!line!
    ENDLOCAL
)

SET searchtxt=D:\\Workspace\\PilotKit\\src\\GUI\\PilotKit.GUI.Web
SET replacetxt=%homedir%\\.dnx\\packages\\PilotKit.GUI.Web\\1.0.0\\root

FOR /f "delims=" %%i in ('TYPE "%textFile%" ^& BREAK ^> "%textFile%"') do (
    SET "line=%%i"
    SETLOCAL enabledelayedexpansion
    SET "line=!line:%searchtxt%=%replacetxt%!"
    >> "%textFile%" ECHO(!line!
    ENDLOCAL
)

