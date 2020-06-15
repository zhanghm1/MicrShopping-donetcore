#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MicrShopping.Identity/MicrShopping.Identity.csproj", "MicrShopping.Identity/"]
COPY ["MicrShopping.Infrastructure.EFCore/MicrShopping.Infrastructure.EFCore.csproj", "MicrShopping.Infrastructure.EFCore/"]
COPY ["MicrShopping.Domain/MicrShopping.Domain.csproj", "MicrShopping.Domain/"]
COPY ["MicrShopping.Infrastructure.Common/MicrShopping.Infrastructure.Common.csproj", "MicrShopping.Infrastructure.Common/"]
RUN dotnet restore "MicrShopping.Identity/MicrShopping.Identity.csproj"
COPY . .
WORKDIR "/src/MicrShopping.Identity"
RUN dotnet build "MicrShopping.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicrShopping.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicrShopping.Identity.dll"]