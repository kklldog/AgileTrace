FROM microsoft/dotnet:latest AS build
WORKDIR /app
COPY /. /app
RUN dotnet restore
WORKDIR /app/AgileTrace.Website
RUN dotnet publish -o ./out -c Release
EXPOSE 5000
ENTRYPOINT ["dotnet", "out/AgileTrace.Website.dll"]
