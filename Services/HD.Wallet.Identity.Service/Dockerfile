FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8300
ENV ASPNETCORE_URLS=http://+:8300

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/HD.Wallet.Identity.Service/HD.Wallet.Identity.Service.csproj", "Services/HD.Wallet.Identity.Service/"]
RUN dotnet restore "Services/HD.Wallet.Identity.Service/HD.Wallet.Identity.Service.csproj"


COPY . .
WORKDIR /src
RUN dotnet build "Services/HD.Wallet.Identity.Service/HD.Wallet.Identity.Service.csproj" -c Release -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Services/HD.Wallet.Identity.Service/HD.Wallet.Identity.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HD.Wallet.Identity.Service.dll"]