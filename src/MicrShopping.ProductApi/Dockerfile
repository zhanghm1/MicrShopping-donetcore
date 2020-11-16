#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt update && apt install -y tzdata
ENV TZ Asia/Shanghai

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicrShopping.ProductApi/MicrShopping.ProductApi.csproj", "MicrShopping.ProductApi/"]
COPY ["MicrShopping.Domain/MicrShopping.Domain.csproj", "MicrShopping.Domain/"]
COPY ["MicrShopping.Infrastructure.Common/MicrShopping.Infrastructure.Common.csproj", "MicrShopping.Infrastructure.Common/"]
RUN dotnet restore "MicrShopping.ProductApi/MicrShopping.ProductApi.csproj"
COPY . .
WORKDIR "/src/MicrShopping.ProductApi"
RUN dotnet build "MicrShopping.ProductApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicrShopping.ProductApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicrShopping.ProductApi.dll"]