FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Unknown.DataAccess/Unknown.DataAccess.csproj", "Unknown.DataAccess/"]
RUN dotnet restore "Unknown.DataAccess/Unknown.DataAccess.csproj"
COPY . .
WORKDIR "/src/Unknown.DataAccess"
RUN dotnet build "Unknown.DataAccess.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unknown.DataAccess.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unknown.DataAccess.dll"]
