FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["GoWeb/GoWeb.csproj", "GoWeb/"]
COPY ["GoWeb.Shared/GoWeb.Shared.csproj", "GoWeb.Shared/"]
COPY ["GoWebApplication.Db/GoWebApplication.Db.csproj", "GoWebApplication.Db/"]

RUN dotnet restore "GoWeb/GoWeb.csproj"


COPY . .


RUN dotnet publish "GoWeb/GoWeb.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GoWeb.dll"]