﻿FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY . .
RUN dotnet restore 
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8070
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Store.Presentation.dll"]