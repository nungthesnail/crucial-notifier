FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Observer.App/Observer.App.csproj", "Observer.App/"]
COPY ["Observer.Common/Observer.Common.csproj", "Observer.Common/"]
COPY ["Observer.EntityFramework/Observer.EntityFramework.csproj", "Observer.EntityFramework/"]
RUN dotnet restore "Observer.App/Observer.App.csproj"
COPY . .
WORKDIR "/src/Observer.App"
RUN dotnet build "Observer.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Observer.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Observer.App.dll"]
