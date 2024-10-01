FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 8200
ENV ASPNETCORE_URLS=http://+:8200

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src


COPY ["HD.Wallet.Shared/HD.Wallet.Shared.csproj", "HD.Wallet.Shared/"]
RUN dotnet restore "HD.Wallet.Shared/HD.Wallet.Shared.csproj"

COPY ["Services/HD.Wallet.Transaction.Service/HD.Wallet.Transaction.Service.csproj", "Services/HD.Wallet.Transaction.Service/"]
RUN dotnet restore "HD.Wallet.Shared/HD.Wallet.Shared.csproj"
RUN dotnet restore "Services/HD.Wallet.Transaction.Service/HD.Wallet.Transaction.Service.csproj"


COPY . .
WORKDIR /src

RUN dotnet build "HD.Wallet.Shared/HD.Wallet.Shared.csproj" -c Release -o /app/build
RUN dotnet build "Services/HD.Wallet.Transaction.Service/HD.Wallet.Transaction.Service.csproj" -c Release -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Services/HD.Wallet.Transaction.Service/HD.Wallet.Transaction.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HD.Wallet.Transaction.Service.dll"]