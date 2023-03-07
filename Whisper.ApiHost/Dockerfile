FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Whisper.ApiHost/Whisper.ApiHost.csproj", "Whisper.ApiHost/"]
COPY ["Eqn.Core/Eqn.Core.csproj", "Eqn.Core/"]
COPY ["Eqn.Storage.FileSystem/Eqn.Storage.FileSystem.csproj", "Eqn.Storage.FileSystem/"]
COPY ["Eqn.MultiTenancy/Eqn.MultiTenancy.csproj", "Eqn.MultiTenancy/"]
COPY ["Eqn.Data/Eqn.Data.csproj", "Eqn.Data/"]
COPY ["Eqn.EventBus.Abstraction/Eqn.EventBus.Abstraction.csproj", "Eqn.EventBus.Abstraction/"]
COPY ["Eqn.ObjectExtending/Eqn.ObjectExtending.csproj", "Eqn.ObjectExtending/"]
COPY ["Eqn.Localization.Abstraction/Eqn.Localization.Abstraction.csproj", "Eqn.Localization.Abstraction/"]
COPY ["Eqn.Validation.Abstraction/Eqn.Validation.Abstraction.csproj", "Eqn.Validation.Abstraction/"]
COPY ["Eqn.Uow/Eqn.Uow.csproj", "Eqn.Uow/"]
COPY ["Eqn.Security/Eqn.Security.csproj", "Eqn.Security/"]
COPY ["Eqn.Storage/Eqn.Storage.csproj", "Eqn.Storage/"]
COPY ["Eqn.Threading/Eqn.Threading.csproj", "Eqn.Threading/"]
COPY ["Eqn.EventBus.RabbitMq/Eqn.EventBus.RabbitMq.csproj", "Eqn.EventBus.RabbitMq/"]
COPY ["Eqn.EventBus/Eqn.EventBus.csproj", "Eqn.EventBus/"]
COPY ["Eqn.BackGroundWorkers/Eqn.BackGroundWorkers.csproj", "Eqn.BackGroundWorkers/"]
COPY ["Eqn.DistributedLocking.Abstraction/Eqn.DistributedLocking.Abstraction.csproj", "Eqn.DistributedLocking.Abstraction/"]
COPY ["Eqn.Guids/Eqn.Guids.csproj", "Eqn.Guids/"]
COPY ["Eqn.Json/Eqn.Json.csproj", "Eqn.Json/"]
COPY ["Eqn.Json.SystemTextJson/Eqn.Json.SystemTextJson.csproj", "Eqn.Json.SystemTextJson/"]
COPY ["Eqn.Json.Abstraction/Eqn.Json.Abstraction.csproj", "Eqn.Json.Abstraction/"]
COPY ["Eqn.Timing/Eqn.Timing.csproj", "Eqn.Timing/"]
COPY ["Eqn.Localization/Eqn.Localization.csproj", "Eqn.Localization/"]
COPY ["Eqn.Settings/Eqn.Settings.csproj", "Eqn.Settings/"]
COPY ["Eqn.VirtualFileSysem/Eqn.VirtualFileSysem.csproj", "Eqn.VirtualFileSysem/"]
COPY ["Eqn.RabbitMq/Eqn.RabbitMq.csproj", "Eqn.RabbitMq/"]
COPY ["Equinox.Shared.Gateway/Equinox.Shared.Gateway.csproj", "Equinox.Shared.Gateway/"]
COPY ["Equinox.Shared.Hosting/Equinox.Shared.Hosting.csproj", "Equinox.Shared.Hosting/"]
COPY ["Eqn.AspNetCore.SeriLog/Eqn.AspNetCore.SeriLog.csproj", "Eqn.AspNetCore.SeriLog/"]
COPY ["Eqn.AspNetCore/Eqn.AspNetCore.csproj", "Eqn.AspNetCore/"]
COPY ["Eqn.Auditing/Eqn.Auditing.csproj", "Eqn.Auditing/"]
COPY ["Eqn.Auditing.Contracts/Eqn.Auditing.Contracts.csproj", "Eqn.Auditing.Contracts/"]
COPY ["Eqn.Authorization/Eqn.Authorization.csproj", "Eqn.Authorization/"]
COPY ["Eqn.Authorization.Abstractions/Eqn.Authorization.Abstractions.csproj", "Eqn.Authorization.Abstractions/"]
COPY ["Eqn.ExceptionHandling/Eqn.ExceptionHandling.csproj", "Eqn.ExceptionHandling/"]
COPY ["Eqn.Http/Eqn.Http.csproj", "Eqn.Http/"]
COPY ["Eqn.Http.Abstraction/Eqn.Http.Abstraction.csproj", "Eqn.Http.Abstraction/"]
COPY ["Eqn.Validations/Eqn.Validations.csproj", "Eqn.Validations/"]
COPY ["Eqn.Autofac/Eqn.Autofac.csproj", "Eqn.Autofac/"]
COPY ["Eqn.Castle.Core/Eqn.Castle.Core.csproj", "Eqn.Castle.Core/"]
COPY ["Eqn.Swashbuckle/Eqn.Swashbuckle.csproj", "Eqn.Swashbuckle/"]
COPY ["Eqn.AspNetCore.Mvc/Eqn.AspNetCore.Mvc.csproj", "Eqn.AspNetCore.Mvc/"]
COPY ["Eqn.ApiVersioning.Abstraction/Eqn.ApiVersioning.Abstraction.csproj", "Eqn.ApiVersioning.Abstraction/"]
COPY ["Eqn.AspNetCore.Mvc.Contracts/Eqn.AspNetCore.Mvc.Contracts.csproj", "Eqn.AspNetCore.Mvc.Contracts/"]
COPY ["Eqn.Ddd.Application.Contracts/Eqn.Ddd.Application.Contracts.csproj", "Eqn.Ddd.Application.Contracts/"]
COPY ["Eqn.Ddd.Application/Eqn.Ddd.Application.csproj", "Eqn.Ddd.Application/"]
COPY ["Eqn.Ddd.Domain/Eqn.Ddd.Domain.csproj", "Eqn.Ddd.Domain/"]
COPY ["Eqn.Caching/Eqn.Caching.csproj", "Eqn.Caching/"]
COPY ["Eqn.Serialization/Eqn.Serialization.csproj", "Eqn.Serialization/"]
COPY ["Eqn.ObjectMapper/Eqn.ObjectMapper.csproj", "Eqn.ObjectMapper/"]
COPY ["Eqn.Specifications/Eqn.Specifications.csproj", "Eqn.Specifications/"]
COPY ["Eqn.Features/Eqn.Features.csproj", "Eqn.Features/"]
COPY ["Eqn.GlobalFeatures/Eqn.GlobalFeatures.csproj", "Eqn.GlobalFeatures/"]
COPY ["Equinox.Shared/Equinox.Shared.csproj", "Equinox.Shared/"]
COPY ["Whisper.Application/Whisper.Application.csproj", "Whisper.Application/"]
COPY ["Whisper.Background/Whisper.Background.csproj", "Whisper.Background/"]
COPY ["Whisper.Domain/Whisper.Domain.csproj", "Whisper.Domain/"]
COPY ["Eqn.AspNetCore.SignalR/Eqn.AspNetCore.SignalR.csproj", "Eqn.AspNetCore.SignalR/"]
RUN dotnet restore "Whisper.ApiHost/Whisper.ApiHost.csproj"
COPY . .
WORKDIR "/src/Whisper.ApiHost"
RUN dotnet dev-certs https --trust
RUN dotnet build "Whisper.ApiHost.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Whisper.ApiHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Whisper.ApiHost.dll"]
