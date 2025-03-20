FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /src

COPY  PostService/*.sln ./PostService/

COPY PostService/PostService.Domain/*.csproj ./PostService/PostService.Domain/
COPY PostService/PostService.Infrastructure/*.csproj ./PostService/PostService.Infrastructure/
COPY PostService/PostService.Host/*.csproj ./PostService/PostService.Host/
COPY BaseLibrary/*.csproj ./BaseLibrary/

RUN dotnet restore ./PostService/PostService.sln

COPY PostService/PostService.Domain/. ./PostService/PostService.Domain/
COPY PostService/PostService.Infrastructure/. ./PostService/PostService.Infrastructure/
COPY PostService/PostService.Host/. ./PostService/PostService.Host/
COPY BaseLibrary/. ./BaseLibrary/

RUN dotnet build ./PostService/PostService.sln

WORKDIR /src/PostService/PostService.Host

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app .

EXPOSE 5230
ENTRYPOINT dotnet PostService.Host.dll