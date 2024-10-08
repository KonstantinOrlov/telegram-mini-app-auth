FROM registry.neftm.local/dotnet/sdk:8.0 as build
## up to src folder
COPY . /src
WORKDIR /src

RUN dotnet restore --source http://nuget.neftm.local/nuget --source https://api.nuget.org/v3/index.json
RUN dotnet publish ./src/TestWebApi/TestWebApi.csproj -c Release -o ./out --no-restore
#----------------------------------------------
FROM registry.neftm.local/dotnet/aspnet:8.0 as prod
WORKDIR /app
COPY --from=build /src/out .

VOLUME ./appsettings ./app/appsettings
VOLUME ./logs ./app/logs
VOLUME ./wwwroot ./app/wwwroot

EXPOSE 80
#EXPOSE 443

ENTRYPOINT [ "dotnet", "TestWebApi.dll", "--urls http://*:80" ]
