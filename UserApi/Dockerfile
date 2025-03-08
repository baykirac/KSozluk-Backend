FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY src/back-end/UserApi ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /app

COPY --from=build /app/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "UserApi.dll"]

# docker build -t core-user-api .

# docker run -d -p 6001:80 --name core-user-api-container core-user-api
