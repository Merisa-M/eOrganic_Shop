FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish "eOrganicShop/eOrganicShop.csproj" -c Release -o /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ADD ./eOrganicShop/Files ./Files



ENTRYPOINT ["dotnet", "eOrganicShop.dll"]