FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DevTalk.API/DevTalk.API.csproj", "src/DevTalk.API/"]
COPY ["src/DevTalk.Application/DevTalk.Application.csproj", "src/DevTalk.Application/"]
COPY ["src/DevTalk.Domain/DevTalk.Domain.csproj", "src/DevTalk.Domain/"]
COPY ["src/DevTalk.Infrastructure/DevTalk.Infrastructure.csproj", "src/DevTalk.Infrastructure/"]
RUN dotnet restore "src/DevTalk.API/DevTalk.API.csproj"
COPY . .
WORKDIR "/src/src/DevTalk.API"
RUN dotnet build "DevTalk.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DevTalk.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevTalk.API.dll"]
