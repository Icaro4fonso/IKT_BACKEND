FROM mcr.microsoft.com/dotnet/nightly/sdk:10.0 AS build
WORKDIR /app

COPY src/IKT_BACKEND.csproj src/
RUN dotnet restore src/IKT_BACKEND.csproj

COPY . .

WORKDIR /app/src
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

EXPOSE 8080

COPY --from=build /out .

ENTRYPOINT ["dotnet", "IKT_BACKEND.dll"]
