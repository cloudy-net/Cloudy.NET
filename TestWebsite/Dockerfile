FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /App

COPY ./Cloudy.CMS ./Cloudy.CMS
COPY ./Cloudy.CMS.UI ./Cloudy.CMS.UI
COPY ./Cloudy.CMS.UI.AzureMediaPicker ./Cloudy.CMS.UI.AzureMediaPicker
COPY ./TestWebsite ./TestWebsite

WORKDIR /App/TestWebsite

RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App/TestWebsite
COPY --from=build-env /App/TestWebsite/out .
ENTRYPOINT ["dotnet", "TestWebsite.dll"]

EXPOSE 80