FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/MqttDumperService/MqttDumperService.csproj", "MqttDumperService/"]
COPY ["src/MqttDumper.Common/MqttDumper.Common.csproj", "MqttDumper.Common/"]

RUN dotnet restore "MqttDumperService/MqttDumperService.csproj"

COPY ["src/MqttDumperService/", "MqttDumperService/"]
COPY ["src/MqttDumper.Common/", "MqttDumper.Common/"]

WORKDIR "/src/MqttDumperService"

RUN dotnet build "MqttDumperService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MqttDumperService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MqttDumperService.dll"]
