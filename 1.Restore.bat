
dotnet restore src/8.Infrastructure/PilotKit.Infrastructure.Interfaces
dotnet build   src/8.Infrastructure/PilotKit.Infrastructure.Interfaces
pause
dotnet restore src/8.Infrastructure/PilotKit.Infrastructure.CrossCutting
dotnet build   src/8.Infrastructure/PilotKit.Infrastructure.CrossCutting
pause
dotnet restore src/5.Domain/PilotKit.Domain.Model
dotnet build   src/5.Domain/PilotKit.Domain.Model
pause
dotnet restore src/4.Repository/PilotKit.Repository.Interfaces
dotnet build   src/4.Repository/PilotKit.Repository.Interfaces
pause
dotnet restore src/4.Repository/PilotKit.Repository.DataAccess
dotnet build   src/4.Repository/PilotKit.Repository.DataAccess
pause
dotnet restore src/5.Domain/PilotKit.Domain.Interfaces
dotnet build   src/5.Domain/PilotKit.Domain.Interfaces
pause
dotnet restore src/5.Domain/PilotKit.Domain.Services
dotnet build   src/5.Domain/PilotKit.Domain.Services
pause
dotnet restore src/6.Orchestration/PilotKit.Orchestration.Interfaces
dotnet build   src/6.Orchestration/PilotKit.Orchestration.Interfaces
pause
dotnet restore src/6.Orchestration/PilotKit.Orchestration.Concrete
dotnet build   src/6.Orchestration/PilotKit.Orchestration.Concrete
pause
dotnet restore src/7.GUI/PilotKit.GUI.Web
dotnet build   src/7.GUI/PilotKit.GUI.Web
pause