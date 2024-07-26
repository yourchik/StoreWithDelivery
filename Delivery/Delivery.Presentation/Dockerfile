FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /Src

COPY . ./
RUN dotnet restore "./Delivery/Delivery.Presentation/Delivery.Presentation.csproj"
RUN dotnet publish "Delivery.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine
WORKDIR /App
EXPOSE 80
COPY --from=publish /App/publish .
ENTRYPOINT ["dotnet", "Store.Presentation.dll"]