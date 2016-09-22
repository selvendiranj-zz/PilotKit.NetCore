set appBasePath=D:\Workspace\PilotKit\src
set dnxToolPath=%userprofile%\.dnx\runtimes\dnx-clr-win-x86.1.0.0-rc1-update2\bin\lib\Microsoft.Dnx.Tooling
set dnxToolName=%dnxToolPath%\Microsoft.Dnx.Tooling.dll

dnx "%dnxToolName%" restore "%appBasePath%\Infrastructure\PilotKit.Infrastructure.Interfaces"   
dnx "%dnxToolName%" restore "%appBasePath%\Domain\PilotKit.Domain.Model"                        
dnx "%dnxToolName%" restore "%appBasePath%\Infrastructure\PilotKit.Infrastructure.CrossCutting" 
dnx "%dnxToolName%" restore "%appBasePath%\Repository\PilotKit.Repository.Interfaces"           
dnx "%dnxToolName%" restore "%appBasePath%\Domain\PilotKit.Domain.Interfaces"                   
dnx "%dnxToolName%" restore "%appBasePath%\Orchestration\PilotKit.Orchestration.Interfaces"     
dnx "%dnxToolName%" restore "%appBasePath%\Repository\PilotKit.Repository.DataAccess"           
dnx "%dnxToolName%" restore "%appBasePath%\Domain\PilotKit.Domain.Services"                     
dnx "%dnxToolName%" restore "%appBasePath%\Orchestration\PilotKit.Orchestration.Concrete"       
dnx "%dnxToolName%" restore "%appBasePath%\GUI\PilotKit.GUI.Web"                                

pause