FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Bulwark.Auth.Admin/Bulwark.Auth.Admin.csproj", "src/Bulwark.Auth.Admin/"]
COPY ["src/Bulwark.Auth.Admin.Repositories/Bulwark.Auth.Admin.Repositories.csproj", "src/Bulwark.Auth.Admin.Repositories/"]
RUN dotnet restore "src/Bulwark.Auth.Admin/Bulwark.Auth.Admin.csproj"
COPY . .
WORKDIR "/src/src/Bulwark.Auth.Admin"
RUN dotnet build "Bulwark.Auth.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bulwark.Auth.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bulwark.Auth.Admin.dll"]