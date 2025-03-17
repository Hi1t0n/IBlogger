FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /src

COPY UserService/*.sln ./UserService/

COPY UserService/UserService.Domain/*.csproj ./UserService/UserService.Domain/
COPY UserService/UserService.Infrastructure/*.csproj ./UserService/UserService.Infrastructure/
COPY UserService/UserService.Host/*.csproj ./UserService/UserService.Host/
COPY BaseLibrary/*.csproj ./BaseLibrary/

RUN dotnet restore ./UserService/UserService.sln

COPY UserService/UserService.Domain/. ./UserService/UserService.Domain/
COPY UserService/UserService.Infrastructure/. ./UserService/UserService.Infrastructure/
COPY UserService/UserService.Host/. ./UserService/UserService.Host/
COPY BaseLibrary/. ./BaseLibrary/

RUN dotnet build ./UserService/UserService.sln

WORKDIR /src/UserService/UserService.Host

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app .

EXPOSE 5229
ENTRYPOINT dotnet UserService.Host.dll