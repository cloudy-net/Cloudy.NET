FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet remove reference ../Cloudy.CMS.AspNetCore/Cloudy.CMS.AspNetCore.csproj
RUN dotnet remove reference ../Cloudy.CMS.UI/Cloudy.CMS.UI.csproj
RUN dotnet restore
RUN dotnet add package Cloudy.CMS.AspNetCore
RUN dotnet add package Cloudy.CMS.UI

COPY . ./
RUN dotnet remove reference ../Cloudy.CMS.AspNetCore/Cloudy.CMS.AspNetCore.csproj
RUN dotnet remove reference ../Cloudy.CMS.UI/Cloudy.CMS.UI.csproj
RUN dotnet add package Cloudy.CMS.AspNetCore
RUN dotnet add package Cloudy.CMS.UI
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Website.AspNetCore.dll"]
