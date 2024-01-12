FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuickJob.Api.Consumer/QuickJob.Api.Consumer.csproj", "QuickJob.Api.Consumer/"]
RUN dotnet restore "QuickJob.Api.Consumer/QuickJob.Api.Consumer.csproj"
COPY . .
WORKDIR "/src/QuickJob.Api.Consumer"
RUN dotnet build "QuickJob.Api.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickJob.Api.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickJob.Api.Consumer.dll"]
