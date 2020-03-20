#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.azk8s.cn/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.azk8s.cn/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MicrShopping.OrderApi/MicrShopping.OrderApi.csproj", "src/MicrShopping.OrderApi/"]
COPY ["src/MicrShopping.Domain/MicrShopping.Domain.csproj", "src/MicrShopping.Domain/"]
RUN dotnet restore "src/MicrShopping.OrderApi/MicrShopping.OrderApi.csproj"
COPY . .
WORKDIR "/src/src/MicrShopping.OrderApi"
RUN dotnet build "MicrShopping.OrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicrShopping.OrderApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicrShopping.OrderApi.dll"]