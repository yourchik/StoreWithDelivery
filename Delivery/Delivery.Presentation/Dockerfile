FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
ENV DOTNET_ENVIRONMENT=Production
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Delivery.Presentation.dll"]


