# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj first for caching
COPY UserApi.csproj ./
RUN dotnet restore UserApi.csproj

# copy everything else in this folder
COPY . ./

# publish
RUN dotnet publish UserApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserApi.dll"]