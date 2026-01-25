# Multi-stage build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar arquivos de projeto
COPY ["src/ApiDapperClean.Api/ApiDapperClean.Api.csproj", "src/ApiDapperClean.Api/"]
COPY ["src/ApiDapperClean.Application/ApiDapperClean.Application.csproj", "src/ApiDapperClean.Application/"]
COPY ["src/ApiDapperClean.Domain/ApiDapperClean.Domain.csproj", "src/ApiDapperClean.Domain/"]
COPY ["src/ApiDapperClean.Infrastructure/ApiDapperClean.Infrastructure.csproj", "src/ApiDapperClean.Infrastructure/"]
COPY ["src/ApiDapperClean.CrossCutting/ApiDapperClean.CrossCutting.csproj", "src/ApiDapperClean.CrossCutting/"]

# Restaurar dependências
RUN dotnet restore "src/ApiDapperClean.Api/ApiDapperClean.Api.csproj"

# Copiar código-fonte
COPY . .

# Compilar
RUN dotnet build "src/ApiDapperClean.Api/ApiDapperClean.Api.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "src/ApiDapperClean.Api/ApiDapperClean.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ApiDapperClean.Api.dll"]
