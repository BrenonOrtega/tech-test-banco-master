#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TechTest.BancoMaster.Travels.Api/TechTest.BancoMaster.Travels.Api.csproj", "src/TechTest.BancoMaster.Travels.Api/"]
COPY ["src/TechTest.BancoMaster.Travels.Application/TechTest.BancoMaster.Travels.Application.csproj", "src/TechTest.BancoMaster.Travels.Application/"]
COPY ["src/TechTest.BancoMaster.Travels.Domain/TechTest.BancoMaster.Travels.Domain.csproj", "src/TechTest.BancoMaster.Travels.Domain/"]
COPY ["src/TechTest.BancoMaster.Travels.Infra/TechTest.BancoMaster.Travels.Infra.csproj", "src/TechTest.BancoMaster.Travels.Infra/"]
RUN dotnet restore "src/TechTest.BancoMaster.Travels.Api/TechTest.BancoMaster.Travels.Api.csproj"
COPY . .
WORKDIR "/src/src/TechTest.BancoMaster.Travels.Api"
RUN dotnet build "TechTest.BancoMaster.Travels.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechTest.BancoMaster.Travels.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechTest.BancoMaster.Travels.Api.dll"]