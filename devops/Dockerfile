ARG VERSION=3.0.100-alpine3.9
FROM mcr.microsoft.com/dotnet/core/sdk:$VERSION AS build
WORKDIR /src
COPY ["/src/Tenogy.Auth/AuthServer/AuthServer.csproj", "app/"]
RUN dotnet restore --disable-parallel "app/AuthServer.csproj"
COPY ["/src/Tenogy.Auth/AuthServer/", "app/"]
WORKDIR /src/app
RUN dotnet publish "AuthServer.csproj" -c Release -o /app

FROM tenogy/blank AS src
WORKDIR /app
COPY --from=build /app ./
