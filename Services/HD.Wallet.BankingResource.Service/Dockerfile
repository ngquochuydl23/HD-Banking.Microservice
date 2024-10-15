FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8400
ENV ASPNETCORE_URLS=http://+:8400

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HD.Wallet.Shared/HD.Wallet.Shared.csproj", "HD.Wallet.Shared/"]
RUN dotnet restore "HD.Wallet.Shared/HD.Wallet.Shared.csproj"

COPY ["Services/HD.Wallet.BankingResource.Service/HD.Wallet.BankingResource.Service.csproj", "Services/HD.Wallet.BankingResource.Service/"]
RUN dotnet restore "HD.Wallet.Shared/HD.Wallet.Shared.csproj"
RUN dotnet restore "Services/HD.Wallet.BankingResource.Service/HD.Wallet.BankingResource.Service.csproj"

COPY . .

WORKDIR /src

RUN dotnet build "HD.Wallet.Shared/HD.Wallet.Shared.csproj" -c Release -o /app/build
RUN dotnet build "Services/HD.Wallet.BankingResource.Service/HD.Wallet.BankingResource.Service.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Services/HD.Wallet.BankingResource.Service/HD.Wallet.BankingResource.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Sao chép thư mục Data vào thư mục publish
COPY Services/HD.Wallet.BankingResource.Service/Data /app/publish/Data

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HD.Wallet.BankingResource.Service.dll"]
