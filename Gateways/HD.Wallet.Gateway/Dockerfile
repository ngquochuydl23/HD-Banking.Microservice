FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8000

ENV ASPNETCORE_URLS=http://+:8000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateways/HD.Wallet.Gateway/HD.Wallet.Gateway.csproj", "Gateways/HD.Wallet.Gateway/"]
RUN dotnet restore "./Gateways/HD.Wallet.Gateway/HD.Wallet.Gateway.csproj"
COPY . .
WORKDIR "/src/Gateways/HD.Wallet.Gateway"
RUN dotnet build "./HD.Wallet.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HD.Wallet.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HD.Wallet.Gateway.dll"]