FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY To-do-list.csproj .
RUN dotnet restore To-do-list.csproj

COPY . .
RUN dotnet publish To-do-list.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_gcServer=1

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet To-do-list.dll --urls http://0.0.0.0:${PORT:-8080}"]
