FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /App
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /App
COPY ./ ./
WORKDIR /App/TestWebsite
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM node:18 as buildjs
WORKDIR /App
COPY ./Cloudy.CMS.UI ./
WORKDIR /App/wwwroot-src
RUN npm ci
RUN npm install

FROM base AS final
WORKDIR /App
COPY --from=build /App/TestWebsite/out .
COPY --from=buildjs /App/wwwroot ./wwwroot/_content/Cloudy.CMS.UI/
ENTRYPOINT ["dotnet", "TestWebsite.dll"]