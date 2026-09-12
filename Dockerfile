FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["sanayı randevu.csproj", "./"]
RUN dotnet restore "sanayı randevu.csproj"
COPY . .
RUN dotnet publish "sanayı randevu.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sanayı randevu.dll"]
