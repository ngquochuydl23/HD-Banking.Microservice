FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 8100
ENV ASPNETCORE_URLS=http://+:8100

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src


COPY ["HD.Wallet.Shared/HD.Wallet.Shared.csproj", "HD.Wallet.Shared/"]
RUN dotnet restore "HD.Wallet.Shared/HD.Wallet.Shared.csproj"

COPY ["Services/HD.Wallet.Account.Service/HD.Wallet.Account.Service.csproj", "Services/HD.Wallet.Account.Service/"]
RUN dotnet restore "HD.Wallet.Shared/HD.Wallet.Shared.csproj"
RUN dotnet restore "Services/HD.Wallet.Account.Service/HD.Wallet.Account.Service.csproj"


COPY . .
WORKDIR /src

RUN dotnet build "HD.Wallet.Shared/HD.Wallet.Shared.csproj" -c Release -o /app/build
RUN dotnet build "Services/HD.Wallet.Account.Service/HD.Wallet.Account.Service.csproj" -c Release -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Services/HD.Wallet.Account.Service/HD.Wallet.Account.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HD.Wallet.Account.Service.dll"]