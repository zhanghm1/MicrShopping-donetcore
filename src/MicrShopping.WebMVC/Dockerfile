#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicrShopping.WebMVC/MicrShopping.WebMVC.csproj", "MicrShopping.WebMVC/"]
RUN dotnet restore "MicrShopping.WebMVC/MicrShopping.WebMVC.csproj"
COPY . .
WORKDIR "/src/MicrShopping.WebMVC"
RUN dotnet build "MicrShopping.WebMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicrShopping.WebMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicrShopping.WebMVC.dll"]