# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# =====restore=====

# copy .sln and .csproj files and restore as distinct layers
COPY src/*.sln .
COPY src/Minibank.Core/*.csproj ./Minibank.Core/
COPY src/Minibank.Data/*.csproj ./Minibank.Data/
COPY src/Minibank.Web/*.csproj ./Minibank.Web/
COPY src/Tests/Minibank.Core.Tests/*.csproj ./Tests/Minibank.Core.Tests/
RUN dotnet restore

# copy everything else and build app
COPY src/ .
RUN dotnet build Minibank.Web -c Release

# =====test=====

# run the unit tests
FROM build AS test
WORKDIR /source/Tests/Minibank.Core.Tests
RUN dotnet test --logger trx

# =====publish=====

# publish
FROM build AS publish
WORKDIR /source/Minibank.Web
RUN dotnet publish -c Release --no-build -o /out

# =====runtime=====

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=publish /out ./
# change output logs from json to simple (necessary for .net 6.0)
ENV Logging__Console__FormatterName="Simple"
ENV ASPNETCORE_URLS="http://+:5000"
ENV ASPNETCORE_ENVIRONMENT="Production"
EXPOSE 5000
ENTRYPOINT ["dotnet", "Minibank.Web.dll"]