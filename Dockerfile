FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuickJob.Api/QuickJob.Api.csproj", "QuickJob.Api/"]
RUN dotnet restore "QuickJob.Api/QuickJob.Api.csproj"
COPY . .
WORKDIR "/src/QuickJob.Api"
RUN dotnet build "QuickJob.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickJob.Api.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickJob.Api.dll"]
