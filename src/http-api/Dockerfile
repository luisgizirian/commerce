FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/publish /app

ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_URLS='http://+:5001'
EXPOSE 5001
ENTRYPOINT [ "dotnet", "/app/WebApi.dll" ]