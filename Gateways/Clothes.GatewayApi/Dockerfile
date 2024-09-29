FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 9000

ENV ASPNETCORE_URLS=http://+:9000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateways/Clothes.GatewayApi/Clothes.GatewayApi.csproj", "Gateways/Clothes.GatewayApi/"]
RUN dotnet restore "./Gateways/Clothes.GatewayApi/Clothes.GatewayApi.csproj"
COPY . .
WORKDIR "/src/Gateways/Clothes.GatewayApi"
RUN dotnet build "./Clothes.GatewayApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Clothes.GatewayApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clothes.GatewayApi.dll"]