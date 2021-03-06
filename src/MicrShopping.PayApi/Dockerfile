#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicrShopping.PayApi/MicrShopping.PayApi.csproj", "MicrShopping.PayApi/"]
COPY ["MicrShopping.Domain/MicrShopping.Domain.csproj", "MicrShopping.Domain/"]
COPY ["MicrShopping.Infrastructure.Common/MicrShopping.Infrastructure.Common.csproj", "MicrShopping.Infrastructure.Common/"]
RUN dotnet restore "MicrShopping.PayApi/MicrShopping.PayApi.csproj"
COPY . .
WORKDIR "/src/MicrShopping.PayApi"
RUN dotnet build "MicrShopping.PayApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicrShopping.PayApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicrShopping.PayApi.dll"]