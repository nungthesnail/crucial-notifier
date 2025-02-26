FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Notifier.App/Notifier.App.csproj", "Notifier.App/"]
COPY ["Notifier.Common/Notifier.Common.csproj", "Notifier.Common/"]
RUN dotnet restore "Notifier.App/Notifier.App.csproj"
COPY . .
WORKDIR "/src/Notifier.App"
RUN dotnet build "Notifier.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Notifier.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifier.App.dll"]
