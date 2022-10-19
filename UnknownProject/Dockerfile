FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "Unknown.Api/Unknown.Api.csproj"
RUN dotnet build "Unknown.Api/Unknown.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unknown.Api/Unknown.Api.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unknown.Api.dll"]
