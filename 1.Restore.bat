
dotnet restore src/8.Infrastructure/PilotKit.Infrastructure.Interfaces
dotnet build   src/8.Infrastructure/PilotKit.Infrastructure.Interfaces
REM dotnet pack    src/8.Infrastructure/PilotKit.Infrastructure.Interfaces -o src/bin
dotnet restore src/8.Infrastructure/PilotKit.Infrastructure.CrossCutting
dotnet build   src/8.Infrastructure/PilotKit.Infrastructure.CrossCutting
dotnet restore src/5.Domain/PilotKit.Domain.Model
dotnet build   src/5.Domain/PilotKit.Domain.Model
dotnet restore src/4.Repository/PilotKit.Repository.Interfaces
dotnet build   src/4.Repository/PilotKit.Repository.Interfaces
dotnet restore src/4.Repository/PilotKit.Repository.DataAccess
dotnet build   src/4.Repository/PilotKit.Repository.DataAccess
dotnet restore src/5.Domain/PilotKit.Domain.Interfaces
dotnet build   src/5.Domain/PilotKit.Domain.Interfaces
dotnet restore src/5.Domain/PilotKit.Domain.Services
dotnet build   src/5.Domain/PilotKit.Domain.Services
dotnet restore src/6.Orchestration/PilotKit.Orchestration.Interfaces
dotnet build   src/6.Orchestration/PilotKit.Orchestration.Interfaces
dotnet restore src/6.Orchestration/PilotKit.Orchestration.Concrete
dotnet build   src/6.Orchestration/PilotKit.Orchestration.Concrete
dotnet restore src/7.GUI/PilotKit.GUI.Web
dotnet build   src/7.GUI/PilotKit.GUI.Web
